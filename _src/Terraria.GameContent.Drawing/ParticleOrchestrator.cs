using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.NetModules;
using Terraria.Graphics;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Drawing;

public class ParticleOrchestrator
{
	internal static ParticlePool<FadingParticle> _poolFading = new ParticlePool<FadingParticle>(200, GetNewFadingParticle);

	private static ParticlePool<FadingPlayerShaderParticle> _poolFadingPlayerShader = new ParticlePool<FadingPlayerShaderParticle>(200, GetNewFadingPlayerShaderParticle);

	private static ParticlePool<LittleFlyingCritterParticle> _poolFlies = new ParticlePool<LittleFlyingCritterParticle>(200, GetNewPooFlyParticle);

	private static ParticlePool<LittleFlyingCritterParticle> _natureFlies = new ParticlePool<LittleFlyingCritterParticle>(200, GetNewNatureFlyParticle);

	private static ParticlePool<ItemTransferParticle> _poolItemTransfer = new ParticlePool<ItemTransferParticle>(100, GetNewItemTransferParticle);

	private static ParticlePool<FakeFishParticle> _fakeFish = new ParticlePool<FakeFishParticle>(100, GetNewFakeFishParticle);

	private static ParticlePool<FlameParticle> _poolFlame = new ParticlePool<FlameParticle>(200, GetNewFlameParticle);

	private static ParticlePool<RandomizedFrameParticle> _poolRandomizedFrame = new ParticlePool<RandomizedFrameParticle>(200, GetNewRandomizedFrameParticle);

	private static ParticlePool<PrettySparkleParticle> _poolPrettySparkle = new ParticlePool<PrettySparkleParticle>(200, GetNewPrettySparkleParticle);

	private static ParticlePool<GasParticle> _poolGas = new ParticlePool<GasParticle>(200, GetNewGasParticle);

	private static ParticlePool<BloodyExplosionParticle> _poolBloodyExplosion = new ParticlePool<BloodyExplosionParticle>(200, GetNewBloodyExplosionParticle);

	private static ParticlePool<ShockIconParticle> _poolShockIcon = new ParticlePool<ShockIconParticle>(200, GetNewShockIconParticle);

	public static ParticlePool<ItemTransferParticle> ScreenItemParticles = new ParticlePool<ItemTransferParticle>(100, GetNewItemTransferParticle_ScreenSpace);

	public static ParticlePool<StormLightningParticle> StormLightningParticles = new ParticlePool<StormLightningParticle>(20, GetNewStormLightningParticle);

	private static SlotId[] _mushBoiExplosionSounds = new SlotId[3];

	public static void RequestParticleSpawn(bool clientOnly, ParticleOrchestraType type, ParticleOrchestraSettings settings, int? overrideInvokingPlayerIndex = null)
	{
		settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
		if (overrideInvokingPlayerIndex.HasValue)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)overrideInvokingPlayerIndex.Value;
		}
		SpawnParticlesDirect(type, settings);
		if (!clientOnly && Main.netMode == 1)
		{
			NetManager.Instance.SendToServer(NetParticlesModule.Serialize(type, settings));
		}
	}

	public static void BroadcastParticleSpawn(ParticleOrchestraType type, ParticleOrchestraSettings settings = default(ParticleOrchestraSettings))
	{
		settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
		if (!Main.dedServ)
		{
			SpawnParticlesDirect(type, settings);
		}
		else
		{
			NetManager.Instance.Broadcast(NetParticlesModule.Serialize(type, settings));
		}
	}

	public static void BroadcastOrRequestParticleSpawn(ParticleOrchestraType type, ParticleOrchestraSettings settings = default(ParticleOrchestraSettings))
	{
		settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
		if (!Main.dedServ)
		{
			SpawnParticlesDirect(type, settings);
		}
		if (Main.netMode != 0)
		{
			NetManager.Instance.SendToServerOrBroadcast(NetParticlesModule.Serialize(type, settings));
		}
	}

	private static FadingParticle GetNewFadingParticle()
	{
		return new FadingParticle();
	}

	private static FadingPlayerShaderParticle GetNewFadingPlayerShaderParticle()
	{
		return new FadingPlayerShaderParticle();
	}

	private static LittleFlyingCritterParticle GetNewPooFlyParticle()
	{
		return new LittleFlyingCritterParticle();
	}

	private static LittleFlyingCritterParticle GetNewNatureFlyParticle()
	{
		return new LittleFlyingCritterParticle();
	}

	private static ItemTransferParticle GetNewItemTransferParticle()
	{
		return new ItemTransferParticle();
	}

	private static FakeFishParticle GetNewFakeFishParticle()
	{
		return new FakeFishParticle();
	}

	private static FlameParticle GetNewFlameParticle()
	{
		return new FlameParticle();
	}

	private static RandomizedFrameParticle GetNewRandomizedFrameParticle()
	{
		return new RandomizedFrameParticle();
	}

	private static PrettySparkleParticle GetNewPrettySparkleParticle()
	{
		return new PrettySparkleParticle();
	}

	private static GasParticle GetNewGasParticle()
	{
		return new GasParticle();
	}

	private static BloodyExplosionParticle GetNewBloodyExplosionParticle()
	{
		return new BloodyExplosionParticle();
	}

	private static ShockIconParticle GetNewShockIconParticle()
	{
		return new ShockIconParticle();
	}

	private static ItemTransferParticle GetNewItemTransferParticle_ScreenSpace()
	{
		return new ItemTransferParticle();
	}

	private static StormLightningParticle GetNewStormLightningParticle()
	{
		return new StormLightningParticle();
	}

	public static void SpawnParticlesDirect(ParticleOrchestraType type, ParticleOrchestraSettings settings)
	{
		if (Main.netMode != 2)
		{
			switch (type)
			{
			case ParticleOrchestraType.SporeCloud:
				Spawn_Chlorophyte(settings);
				break;
			case ParticleOrchestraType.Keybrand:
				Spawn_Keybrand(settings);
				break;
			case ParticleOrchestraType.FlameWaders:
				Spawn_FlameWaders(settings);
				break;
			case ParticleOrchestraType.StellarTune:
				Spawn_StellarTune(settings);
				break;
			case ParticleOrchestraType.CattivaHit:
				Spawn_CattivaHit(settings);
				break;
			case ParticleOrchestraType.FormatC:
				Spawn_FormatC(settings);
				break;
			case ParticleOrchestraType.WallOfFleshGoatMountFlames:
				Spawn_WallOfFleshGoatMountFlames(settings);
				break;
			case ParticleOrchestraType.BlackLightningHit:
				Spawn_BlackLightningHit(settings);
				break;
			case ParticleOrchestraType.RainbowRodHit:
				Spawn_RainbowRodHit(settings);
				break;
			case ParticleOrchestraType.BlackLightningSmall:
				Spawn_BlackLightningSmall(settings);
				break;
			case ParticleOrchestraType.StardustPunch:
				Spawn_StardustPunch(settings);
				break;
			case ParticleOrchestraType.PrincessWeapon:
				Spawn_PrincessWeapon(settings);
				break;
			case ParticleOrchestraType.PaladinsHammer:
				Spawn_PaladinsHammer(settings);
				break;
			case ParticleOrchestraType.NightsEdge:
				Spawn_NightsEdge(settings);
				break;
			case ParticleOrchestraType.SilverBulletSparkle:
				Spawn_SilverBulletSparkle(settings);
				break;
			case ParticleOrchestraType.TrueNightsEdge:
				Spawn_TrueNightsEdge(settings);
				break;
			case ParticleOrchestraType.ChlorophyteLeafCrystalPassive:
				Spawn_LeafCrystalPassive(settings);
				break;
			case ParticleOrchestraType.ChlorophyteLeafCrystalShot:
				Spawn_LeafCrystalShot(settings);
				break;
			case ParticleOrchestraType.BestReforge:
				Spawn_BestReforge(settings);
				break;
			case ParticleOrchestraType.TerraBlade:
				Spawn_TerraBlade(settings);
				break;
			case ParticleOrchestraType.Excalibur:
				Spawn_Excalibur(settings);
				break;
			case ParticleOrchestraType.TrueExcalibur:
				Spawn_TrueExcalibur(settings);
				break;
			case ParticleOrchestraType.PetExchange:
				Spawn_PetExchange(settings);
				break;
			case ParticleOrchestraType.SlapHand:
				Spawn_SlapHand(settings);
				break;
			case ParticleOrchestraType.WaffleIron:
				Spawn_WaffleIron(settings);
				break;
			case ParticleOrchestraType.FlyMeal:
				Spawn_FlyMeal(settings);
				break;
			case ParticleOrchestraType.ClassyCane:
				Spawn_ClassyCane(settings);
				break;
			case ParticleOrchestraType.VampireOnFire:
				Spawn_VampireOnFire(settings);
				break;
			case ParticleOrchestraType.GasTrap:
				Spawn_GasTrap(settings);
				break;
			case ParticleOrchestraType.ItemTransfer:
				Spawn_ItemTransfer(settings);
				break;
			case ParticleOrchestraType.ShimmerArrow:
				Spawn_ShimmerArrow(settings);
				break;
			case ParticleOrchestraType.TownSlimeTransform:
				Spawn_TownSlimeTransform(settings);
				break;
			case ParticleOrchestraType.LoadoutChange:
				Spawn_LoadOutChange(settings);
				break;
			case ParticleOrchestraType.ShimmerBlock:
				Spawn_ShimmerBlock(settings);
				break;
			case ParticleOrchestraType.Digestion:
				Spawn_Digestion(settings);
				break;
			case ParticleOrchestraType.PooFly:
				Spawn_PooFly(settings);
				break;
			case ParticleOrchestraType.ShimmerTownNPC:
				Spawn_ShimmerTownNPC(settings);
				break;
			case ParticleOrchestraType.ShimmerTownNPCSend:
				Spawn_ShimmerTownNPCSend(settings);
				break;
			case ParticleOrchestraType.DeadCellsMushroomBoiExplosion:
				Spawn_DeadCellsMushroomBoiExplosion(settings);
				break;
			case ParticleOrchestraType.DeadCellsDownDashExplosion:
				Spawn_DeadCellsDownDashExplosion(settings);
				break;
			case ParticleOrchestraType.DeadCellsBarnacleShotFiring:
				Spawn_DeadCellsBarnacleShotFiring(settings);
				break;
			case ParticleOrchestraType.BlueLightningSmall:
				Spawn_LightningSmall(settings);
				break;
			case ParticleOrchestraType.ShadowOrbExplosion:
				Spawn_ShadowOrbExplosion(settings);
				break;
			case ParticleOrchestraType.UFOLaser:
				Spawn_UFOLaser(settings);
				break;
			case ParticleOrchestraType.DeadCellsBeheadedEffect:
				Spawn_DeadCellsHeadEffect(settings);
				break;
			case ParticleOrchestraType.DeadCellsFlint:
				Spawn_DeadCellsFlint(settings);
				break;
			case ParticleOrchestraType.DeadCellsBarrelExplosion:
				Spawn_DeadCellsBarrelExplosion(settings);
				break;
			case ParticleOrchestraType.DeadCellsMushroomBoiTargetFound:
				Spawn_DeadCellsMushroomBoiTargetFound(settings);
				break;
			case ParticleOrchestraType.MoonLordWhipHit:
				Spawn_MoonLordWhip(settings);
				break;
			case ParticleOrchestraType.MoonLordWhipEye:
				Spawn_MoonLordWhipEye(settings);
				break;
			case ParticleOrchestraType.PlayerVoiceOverrideSound:
				Spawn_PlayerVoiceOverrideSound(settings);
				break;
			case ParticleOrchestraType.RainbowBoulder1:
				Spawn_RainbowBoulder1(settings);
				break;
			case ParticleOrchestraType.RainbowBoulder2:
				Spawn_RainbowBoulder2(settings);
				break;
			case ParticleOrchestraType.RainbowBoulder3:
				Spawn_RainbowBoulder3(settings);
				break;
			case ParticleOrchestraType.RainbowBoulder4:
				Spawn_RainbowBoulder4(settings);
				break;
			case ParticleOrchestraType.FakeFish:
				Spawn_FakeFish(settings);
				break;
			case ParticleOrchestraType.LakeSparkle:
				Spawn_LakeSparkle(settings);
				break;
			case ParticleOrchestraType.NatureFly:
				Spawn_NatureFly(settings);
				break;
			case ParticleOrchestraType.FakeFishJump:
				Spawn_FakeFish(settings, jumping: true);
				break;
			case ParticleOrchestraType.MagnetSphereBolt:
				Spawn_MagnetSphereBolt(settings);
				break;
			case ParticleOrchestraType.HeatRay:
				Spawn_HeatRay(settings);
				break;
			case ParticleOrchestraType.ShadowbeamHostile:
				Spawn_Shadowbeam(settings);
				break;
			case ParticleOrchestraType.ShadowbeamFriendly:
				Spawn_Shadowbeam(settings, hostile: false);
				break;
			case ParticleOrchestraType.RainbowBoulderPetBounce:
				Spawn_RainbowBoulder4(settings, pet: true);
				break;
			case ParticleOrchestraType.StormLightning:
				Spawn_StormLightning(settings);
				break;
			case ParticleOrchestraType.ArcSurge:
				Spawn_ArcLightning(settings);
				break;
			case ParticleOrchestraType.StormlightningWindup:
				Spawn_StormLightningWindup(settings);
				break;
			case ParticleOrchestraType.PaladinsShieldHit:
				Spawn_PaladinsShieldHit(settings);
				break;
			case ParticleOrchestraType.HeroicisSetSpawnSound:
				Spawn_HeroicisSetSpawnSound(settings);
				break;
			case ParticleOrchestraType.BlueLightningSmallLong:
				Spawn_BlueLightningSmallLong(settings);
				break;
			case ParticleOrchestraType.InScreenDungeonSpawn:
				Spawn_InScreenDungeonSpawn(settings);
				break;
			case ParticleOrchestraType.PaladinsHammerShockwave:
				Spawn_PaladinsHammerShockwave(settings);
				break;
			case ParticleOrchestraType.SnappingStone:
				Spawn_SnappingStone(settings);
				break;
			case ParticleOrchestraType.HarpyCharm:
				Spawn_HarpyCharm(settings);
				break;
			case ParticleOrchestraType.SnappingStoneMelee:
				Spawn_SnappingStoneMelee(settings);
				break;
			case ParticleOrchestraType.SnappingStoneSummon:
				Spawn_SnappingStoneSummon(settings);
				break;
			case ParticleOrchestraType.PhoenixQuiver:
				Spawn_HarpyCharm(settings, 6181);
				break;
			case ParticleOrchestraType.RedLightningSmall:
				Spawn_LightningSmall(settings);
				break;
			case ParticleOrchestraType.SeraphNecklace:
				Spawn_HarpyCharm(settings, 6180);
				break;
			case ParticleOrchestraType.SnappingStoneRecharge:
				Spawn_SnappingStoneRecharge(settings);
				break;
			case ParticleOrchestraType.PyroclasticStoneRecharge:
				Spawn_PyroclasticStoneRecharge(settings);
				break;
			case ParticleOrchestraType.ArmletOfRuinRecharge:
				Spawn_ArmletOfRuinRecharge(settings);
				break;
			}
		}
	}

	private static void Spawn_ArmletOfRuinRecharge(ParticleOrchestraSettings settings)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
		for (int i = 0; i < 10; i++)
		{
			int num = Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 255);
			Main.dust[num].noLight = true;
			Main.dust[num].noGravity = true;
			Main.dust[num].scale = 0.9f;
			Main.dust[num].fadeIn = 1.1f;
			Main.dust[num].position.Y = player.position.Y + (float)(player.height / 2);
			Dust obj = Main.dust[num];
			obj.velocity *= 0.5f;
			Main.dust[num].velocity.Y *= 3f;
			Main.dust[num].alpha = 60;
			Main.dust[num].customData = player;
			Main.dust[num].noLightEmittance = true;
		}
	}

	private static void Spawn_PyroclasticStoneRecharge(ParticleOrchestraSettings settings)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
		for (int i = 0; i < 15; i++)
		{
			int num = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 255);
			Main.dust[num].noLight = true;
			Main.dust[num].noGravity = true;
			Main.dust[num].scale = 1.5f;
			Main.dust[num].fadeIn = 1.1f;
			Main.dust[num].position.Y = player.position.Y + (float)(player.height / 2);
			Main.dust[num].velocity = Main.rand.NextVector2Circular(2f, 4f);
			Main.dust[num].customData = player;
			Main.dust[num].noLightEmittance = true;
		}
	}

	private static void Spawn_SnappingStoneRecharge(ParticleOrchestraSettings settings)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
		for (int i = 0; i < 10; i++)
		{
			int num = Dust.NewDust(player.position, player.width, player.height, 226, 0f, 0f, 255);
			Main.dust[num].noLight = true;
			Main.dust[num].noGravity = true;
			Main.dust[num].scale = 0.9f;
			Main.dust[num].fadeIn = 1.1f;
			Main.dust[num].position.Y = player.position.Y + (float)(player.height / 2);
			Dust obj = Main.dust[num];
			obj.velocity *= 0.5f;
			Main.dust[num].velocity.Y *= 3f;
			Main.dust[num].customData = player;
			Main.dust[num].noLightEmittance = true;
		}
	}

	private static void Spawn_HarpyCharm(ParticleOrchestraSettings settings, int itemID = 6166)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		bool flag = itemID == 6181;
		bool flag2 = itemID == 6180;
		Main.instance.LoadItem(320);
		Asset<Texture2D> val = TextureAssets.Item[320];
		LegacySoundStyle type = SoundID.Item39;
		float pitchOffset = 0f;
		float volumeScale = 1f;
		if (flag)
		{
			val = TextureAssets.Extra[306];
			type = SoundID.Item20;
		}
		else if (flag2)
		{
			val = TextureAssets.Extra[309];
		}
		if (!val.IsLoaded)
		{
			return;
		}
		Vector2 positionInWorld = settings.PositionInWorld;
		Vector2 movementVector = settings.MovementVector;
		int num = 30;
		int num2 = 4;
		int num3 = 3;
		float num4 = 1f;
		if (flag)
		{
			num2 *= 2;
			num3 *= 2;
			num4 *= 2f;
		}
		if (flag2)
		{
			num2 *= 2;
			num3 *= 2;
			num4 *= 2f;
		}
		SoundEngine.PlaySound(type, positionInWorld, pitchOffset, volumeScale);
		Color colorTint = new Color(255, 220, 140, 100);
		for (int i = 0; i < num2; i++)
		{
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.ColorTint = new Color(255, 255, 255, 255);
			if (flag2)
			{
				fadingParticle.ColorTint = colorTint;
			}
			fadingParticle.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num, flag | flag2);
			fadingParticle.FadeInNormalizedTime = 0.1f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.Scale = Vector2.One * 0.75f;
			fadingParticle.ScaleVelocity = new Vector2(1f, 1f) * -0.3f / (float)num;
			fadingParticle.LocalPosition = positionInWorld;
			fadingParticle.Velocity = movementVector.RotatedByRandom(6.2831854820251465) * Main.rand.NextFloat() * 0.25f * num4;
			fadingParticle.AccelerationPerFrame = fadingParticle.Velocity / (float)(-num);
			fadingParticle.Rotation = (float)Math.PI * 2f * Main.rand.NextFloat();
			fadingParticle.RotationVelocity = Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) / (float)num;
			fadingParticle.RotationAcceleration = (0f - fadingParticle.RotationVelocity) / (float)num;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}
		for (int j = 0; j < num3; j++)
		{
			FadingParticle fadingParticle2 = _poolFading.RequestParticle();
			fadingParticle2.ColorTint = new Color(255, 255, 255, 255);
			if (flag2)
			{
				fadingParticle2.ColorTint = colorTint;
			}
			fadingParticle2.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
			fadingParticle2.SetTypeInfo(num, flag | flag2);
			fadingParticle2.FadeInNormalizedTime = 0.1f;
			fadingParticle2.FadeOutNormalizedTime = 0.1f;
			fadingParticle2.Scale = Vector2.One * 0.75f;
			fadingParticle2.ScaleVelocity = new Vector2(1f, 1f) * -0.3f / (float)num;
			fadingParticle2.LocalPosition = positionInWorld;
			fadingParticle2.Velocity = movementVector.RotatedByRandom(0.3141592741012573) * Main.rand.NextFloat() * 1f * num4;
			fadingParticle2.AccelerationPerFrame = fadingParticle2.Velocity / (float)(-num);
			fadingParticle2.Rotation = (float)Math.PI * 2f * Main.rand.NextFloat();
			fadingParticle2.RotationVelocity = Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) / (float)num;
			fadingParticle2.RotationAcceleration = (0f - fadingParticle2.RotationVelocity) / (float)num;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
		}
		for (int k = 0; k < 1; k++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			int num5 = Main.rand.Next(20, 40);
			if (flag)
			{
				prettySparkleParticle.ColorTint = new Color(242, 42, 0, 20);
			}
			else if (flag2)
			{
				prettySparkleParticle.ColorTint = new Color(249, 225, 102, 255);
			}
			else
			{
				prettySparkleParticle.ColorTint = Main.hslToRgb(0.65f, 1f, 0.5f, 0);
			}
			prettySparkleParticle.LocalPosition = positionInWorld;
			prettySparkleParticle.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle.Scale = new Vector2(8f, 0.4f);
			prettySparkleParticle.FadeInNormalizedTime = 0.1f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.5f;
			prettySparkleParticle.TimeToLive = num5;
			prettySparkleParticle.FadeOutEnd = num5;
			prettySparkleParticle.FadeInEnd = num5 / 2;
			prettySparkleParticle.FadeOutStart = num5 / 2;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.Velocity = movementVector;
			if (flag | flag2)
			{
				prettySparkleParticle.Velocity *= 0.3f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
	}

	private static void Spawn_SnappingStone(ParticleOrchestraSettings settings)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(1121);
		Asset<Texture2D> val = TextureAssets.Projectile[1121];
		if (!val.IsLoaded)
		{
			return;
		}
		Vector2 positionInWorld = settings.PositionInWorld;
		float num = settings.MovementVector.X / 256f;
		int num2 = (int)settings.MovementVector.Y;
		int num3 = 20;
		SoundEngine.PlaySound(SoundID.Item72, positionInWorld);
		if (num > 0.12f)
		{
			num = 0.12f;
		}
		for (int i = 0; i < 2; i++)
		{
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.ColorTint = new Color(30, 180, 255, 0);
			fadingParticle.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num3);
			fadingParticle.FadeInNormalizedTime = 0.1f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.Scale = Vector2.Zero;
			fadingParticle.ScaleVelocity = new Vector2(6f, 6f) * num / (float)num3;
			fadingParticle.ScaleAcceleration = -fadingParticle.ScaleVelocity / (float)num3;
			fadingParticle.LocalPosition = positionInWorld;
			fadingParticle.Delay = num2 * 3;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}
		for (int j = 0; j < 20; j++)
		{
			bool flag = Main.rand.Next(3) != 0;
			short type = (short)(flag ? 226 : 15);
			Dust dust = Dust.NewDustDirect(positionInWorld, 0, 0, type);
			dust.velocity *= 64f * num;
			dust.noGravity = true;
			dust.scale = 1f;
			dust.fadeIn = 1.5f;
			dust.noLight = true;
			dust.fadeIn = 0f;
			if (flag)
			{
				dust.velocity *= 0.5f;
				if (Main.rand.Next(6) == 0)
				{
					dust.noGravity = false;
					dust.velocity *= 0.5f;
				}
				dust.noLightEmittance = true;
			}
		}
	}

	private static void Spawn_SnappingStoneMelee(ParticleOrchestraSettings settings)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(1131);
		if (!TextureAssets.Projectile[1131].IsLoaded)
		{
			return;
		}
		Vector2 positionInWorld = settings.PositionInWorld;
		_ = settings.MovementVector.X / 256f;
		int num = 30;
		SoundEngine.PlaySound(SoundID.PyroclasticStone, positionInWorld, 0f, 0.5f);
		float num2 = 14f;
		float num3 = 0.15f;
		float num4 = 0f + Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.05f;
		Vector2 val = new Vector2(-4f, 0f);
		Vector2 val2 = new Vector2(8f, 2f);
		Color colorTint = new Color(1f, 0f, 0f, 0.5f);
		Color colorTint2 = new Color(1f, 0.95f, 0f, 1f);
		for (float num5 = 1f; num5 < 2f; num5++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 val3 = ((float)Math.PI / 4f + (float)Math.PI / 4f * num5 + num4).ToRotationVector2() * num2;
			prettySparkleParticle.ColorTint = colorTint;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = val3.ToRotation();
			prettySparkleParticle.Scale = val2 * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2;
			prettySparkleParticle.FadeOutStart = num / 2;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.ScaleAcceleration = val / (float)num;
			prettySparkleParticle.LocalPosition -= val3 * (float)num * num3;
			prettySparkleParticle.Velocity = val3;
			prettySparkleParticle.AccelerationPerFrame = -prettySparkleParticle.Velocity / (float)num;
			prettySparkleParticle.DrawVerticalAxis = false;
			prettySparkleParticle.AccelerationPerFrame = -prettySparkleParticle.Velocity / (float)num;
			if (num5 == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
				prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num6 = 1f; num6 < 2f; num6 += 2f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 val4 = ((float)Math.PI / 4f + (float)Math.PI / 4f * num6 + num4).ToRotationVector2() * num2;
			prettySparkleParticle2.ColorTint = colorTint2;
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = val4.ToRotation();
			prettySparkleParticle2.Scale = val2 * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2;
			prettySparkleParticle2.FadeOutStart = num / 2;
			prettySparkleParticle2.ScaleAcceleration = val / (float)num;
			prettySparkleParticle2.LocalPosition -= val4 * (float)num * num3;
			prettySparkleParticle2.Velocity = val4;
			prettySparkleParticle2.DrawVerticalAxis = false;
			prettySparkleParticle2.AccelerationPerFrame = -prettySparkleParticle2.Velocity / (float)num;
			if (num6 == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
				prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 2; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 6, val4.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.4f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 6, -val4.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_SnappingStoneSummon(ParticleOrchestraSettings settings)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(1132);
		Asset<Texture2D> val = TextureAssets.Projectile[1132];
		if (!val.IsLoaded)
		{
			return;
		}
		Vector2 positionInWorld = settings.PositionInWorld;
		float num = 1f;
		int num2 = (int)settings.MovementVector.Y;
		int num3 = 20;
		SoundEngine.PlaySound(SoundID.ArmletOfRuin, positionInWorld, 0f, 0.5f);
		if (num > 0.12f)
		{
			num = 0.12f;
		}
		Color colorTint = new Color(90, 110, 255, 90);
		for (int i = 0; i < 2; i++)
		{
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.Velocity = Vector2.Zero;
			fadingParticle.ColorTint = new Color(120, 120, 255, 64);
			fadingParticle.ColorTint = colorTint;
			fadingParticle.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num3);
			fadingParticle.FadeInNormalizedTime = 0.1f;
			fadingParticle.FadeOutNormalizedTime = 0.3f;
			fadingParticle.Scale = Vector2.Zero;
			fadingParticle.ScaleVelocity = new Vector2(9f, 9f) * num / (float)num3;
			fadingParticle.ScaleAcceleration = -fadingParticle.ScaleVelocity / (float)num3;
			fadingParticle.LocalPosition = positionInWorld;
			fadingParticle.Delay = num2 * 3;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}
		for (int j = 0; j < 1; j++)
		{
			FadingParticle fadingParticle2 = _poolFading.RequestParticle();
			fadingParticle2.Velocity = Vector2.Zero;
			fadingParticle2.ColorTint = new Color(140, 100, 255, 60);
			fadingParticle2.ColorTint = colorTint;
			fadingParticle2.SetBasicInfo(TextureAssets.Extra[34], null, Vector2.Zero, Vector2.Zero);
			fadingParticle2.SetTypeInfo(num3);
			fadingParticle2.FadeInNormalizedTime = 0.1f;
			fadingParticle2.FadeOutNormalizedTime = 0.5f;
			fadingParticle2.Scale = Vector2.Zero;
			fadingParticle2.ScaleVelocity = new Vector2(9f, 9f) * num / (float)num3;
			fadingParticle2.ScaleAcceleration = -fadingParticle2.ScaleVelocity / (float)num3;
			fadingParticle2.LocalPosition = positionInWorld;
			fadingParticle2.Delay = num2 * 3;
			fadingParticle2.RotationVelocity = Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.33f / 60f;
			fadingParticle2.Rotation = Main.rand.NextFloat() * ((float)Math.PI * 2f);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
		}
		for (int k = 0; k < 15; k++)
		{
			int type = 172;
			float num4 = 10f * num;
			Dust dust = Dust.NewDustPerfect(positionInWorld, type, Main.rand.NextVector2Circular(10f, 10f));
			dust.scale = Math.Max(0.75f, 2f * num4);
			dust.noGravity = true;
			if (Main.rand.Next(6) == 0)
			{
				dust.noGravity = false;
				dust.scale *= 0.25f;
			}
		}
		for (int l = 0; l < 8; l++)
		{
			int type2 = 211;
			float num5 = 10f * num;
			Dust dust2 = Dust.NewDustPerfect(positionInWorld, type2, Main.rand.NextVector2Circular(10f, 10f));
			dust2.scale = Math.Max(0.75f, 2f * num5);
			dust2.noGravity = true;
			if (Main.rand.Next(6) == 0)
			{
				dust2.noGravity = false;
				dust2.scale *= 0.25f;
			}
		}
	}

	private static void Spawn_PaladinsHammerShockwave(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		int num = (int)settings.MovementVector.X;
		int num2 = (int)settings.MovementVector.Y;
		Vector2 val = positionInWorld + new Vector2((float)num * 0.5f, (float)num2 * 0.5f);
		SoundEngine.PlaySound(SoundID.Item180, val);
		for (int i = 0; i < 50; i++)
		{
			if (Main.rand.Next(2) == 0)
			{
				int num3 = Dust.NewDust(positionInWorld, num, num2, 57, 0f, 0f, 200, default, 1.8f);
				Main.dust[num3].velocity = Main.dust[num3].position - val;
				Main.dust[num3].velocity.Normalize();
				Dust obj = Main.dust[num3];
				obj.velocity *= 9.5f;
				Main.dust[num3].noGravity = true;
				Dust obj2 = Main.dust[num3];
				obj2.velocity *= Main.rand.NextFloat();
			}
			if (Main.rand.Next(3) == 0)
			{
				int num4 = Dust.NewDust(positionInWorld, num, num2, 43, 0f, 0f, 254, default, 1.3f);
				Main.dust[num4].velocity = Main.dust[num4].position - val;
				Main.dust[num4].velocity.Normalize();
				Dust obj3 = Main.dust[num4];
				obj3.velocity *= 9.5f;
				Main.dust[num4].noGravity = true;
				Dust obj4 = Main.dust[num4];
				obj4.velocity *= Main.rand.NextFloat();
			}
		}
		int num5 = 4;
		Point val2 = settings.PositionInWorld.ToTileCoordinates();
		for (int j = val2.X - 3; j <= val2.X + 3; j++)
		{
			for (int k = val2.Y - 3; k <= val2.Y + 3; k++)
			{
				Tile tileSafely = Framing.GetTileSafely(j, k);
				if (tileSafely.active())
				{
					int num6 = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely);
					for (int l = 0; l < num6; l++)
					{
						Dust obj5 = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
						obj5.velocity.Y -= 3f + (float)num5 * 1.5f;
						obj5.velocity.Y *= Main.rand.NextFloat();
						obj5.velocity.Y *= 0.75f;
						obj5.scale += (float)num5 * 0.03f;
					}
					for (int m = 0; m < num6 - 1; m++)
					{
						Dust obj6 = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
						obj6.velocity.Y -= 1f + (float)num5;
						obj6.velocity.Y *= Main.rand.NextFloat();
						obj6.velocity.Y *= 0.75f;
					}
					if (num6 > 0 && Main.rand.Next(2) != 0)
					{
						float num7 = Math.Abs(val2.X - j) / 3;
						Gore gore = Gore.NewGoreDirect(val, Vector2.Zero, 61 + Main.rand.Next(3), 1f - (float)num5 * 0.15f + num7 * 0.5f);
						gore.velocity.Y -= 0.1f + (float)num5 * 0.5f + num7 * (float)num5 * 1f;
						gore.velocity.Y *= Main.rand.NextFloat();
						gore.position = new Vector2((float)(j * 16 + 20), (float)(k * 16));
					}
				}
			}
		}
	}

	private static void Spawn_InScreenDungeonSpawn(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		Vector2 movementVector = settings.MovementVector;
		Vector2 positionInWorld = settings.PositionInWorld;
		int uniqueInfoPiece = settings.UniqueInfoPiece;
		Vector2 val = positionInWorld + new Vector2(-0.5f, -1f) * movementVector;
		int num = Main.rand.Next(2, 5);
		for (int i = 0; i < num; i++)
		{
			Gore gore = Gore.NewGorePerfect(val + new Vector2(-16f, -16f) + movementVector * new Vector2(Main.rand.NextFloat(), 0.5f + Main.rand.NextFloat() * 0.5f), Main.rand.NextVector2Circular(2f, 2f), Main.rand.Next(61, 64));
			gore.scale *= 1f;
			gore.position -= gore.velocity * 5f;
		}
		SoundEngine.PlaySound(ContentSamples.NpcsByNetId[uniqueInfoPiece].HitSound, (int)settings.PositionInWorld.X, (int)settings.PositionInWorld.Y);
	}

	private static void Spawn_HeroicisSetSpawnSound(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(SoundID.RainbowBoulder, (int)settings.PositionInWorld.X, (int)settings.PositionInWorld.Y);
	}

	private static void Spawn_PaladinsShieldHit(ParticleOrchestraSettings settings)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadItem(938);
		Asset<Texture2D> val = TextureAssets.Item[938];
		if (!val.IsLoaded)
		{
			return;
		}
		Vector2 positionInWorld = settings.PositionInWorld;
		int num = (int)positionInWorld.X;
		int num2 = (int)positionInWorld.Y;
		Vector2 val2 = Vector2.Zero;
		if (Main.player.IndexInRange(num))
		{
			Player player = Main.player[num];
			val2 = player.MountedCenter;
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			int num3 = 30;
			fadingParticle.followPlayerIndex = num;
			fadingParticle.ColorTint = new Color(255, 255, 255, 220) * 0.85f;
			fadingParticle.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num3);
			fadingParticle.FadeInNormalizedTime = 0.1f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.Scale = new Vector2(1f, 1f) * 0.65f;
			fadingParticle.ScaleVelocity = new Vector2(2f, 2f) / (float)num3;
			fadingParticle.ScaleAcceleration = -fadingParticle.ScaleVelocity / (float)num3;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
			for (int i = 0; i < 4; i++)
			{
				Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 57);
				dust.velocity *= 3f;
				dust.noGravity = true;
				dust.scale = 1f;
				dust.fadeIn = 1.5f;
				dust.noLight = true;
			}
		}
		if (Main.player.IndexInRange(num2))
		{
			Player player2 = Main.player[num2];
			FadingParticle fadingParticle2 = _poolFading.RequestParticle();
			float num4 = 15f;
			fadingParticle2.followPlayerIndex = num;
			fadingParticle2.ColorTint = new Color(255, 255, 255, 127) * 0.75f;
			fadingParticle2.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
			fadingParticle2.SetTypeInfo(num4);
			fadingParticle2.FadeInNormalizedTime = 0.1f;
			fadingParticle2.FadeOutNormalizedTime = 0.1f;
			fadingParticle2.Scale = new Vector2(1f, 1f) * 0.65f;
			fadingParticle2.ScaleVelocity = new Vector2(2f, 2f) / num4;
			fadingParticle2.ScaleAcceleration = -fadingParticle2.ScaleVelocity / num4;
			Vector2 val3 = val2 - player2.MountedCenter;
			fadingParticle2.LocalPosition = -val3;
			if (num == -1)
			{
				fadingParticle2.LocalPosition = player2.MountedCenter;
			}
			fadingParticle2.Velocity = val3 * 0.5f / num4;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
			for (int j = 0; j < 4; j++)
			{
				Dust dust2 = Dust.NewDustDirect(player2.position, player2.width, player2.height, 57);
				dust2.velocity *= 3f;
				dust2.noGravity = true;
				dust2.scale = 1f;
				dust2.fadeIn = 1.5f;
				dust2.noLight = true;
			}
		}
	}

	private static void Spawn_StormLightningWindup(ParticleOrchestraSettings settings)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = Main.rand.Next(1, 3);
		float num3 = 0.7f;
		short num4 = 916;
		Main.instance.LoadProjectile(num4);
		Color white = Color.White;
		if (settings.UniqueInfoPiece != 0)
		{
			white.PackedValue = (uint)settings.UniqueInfoPiece;
		}
		white.A = 0;
		for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
		{
			int num6 = Main.rand.Next(7, 11);
			float f = (float)Math.PI * 2f * num5 + num + Main.rand.NextFloatDirection() * 0.25f;
			float num7 = Main.rand.NextFloat() * 3f + 0.1f;
			Vector2 val = Main.rand.NextVector2Circular(6f, 6f) * num3;
			Color colorTint = white;
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num4], null, Vector2.Zero, val);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num4], 2, num6);
			randomizedFrameParticle.Velocity = f.ToRotationVector2() * num7 * new Vector2(1f, 0.5f) + settings.MovementVector * 0.5f;
			randomizedFrameParticle.ColorTint = colorTint;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + val;
			randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
			randomizedFrameParticle.Scale = new Vector2(1.5f, 0.75f) * 1f;
			randomizedFrameParticle.FadeInNormalizedTime = 1f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0.9f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
			if (Main.rand.Next(3) == 0)
			{
				randomizedFrameParticle.LocalPosition += randomizedFrameParticle.Velocity * (float)(num6 - 1);
				randomizedFrameParticle.Velocity *= -0.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
			RandomizedFrameParticle randomizedFrameParticle2 = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle2.SetBasicInfo(TextureAssets.Projectile[num4], null, Vector2.Zero, val);
			randomizedFrameParticle2.SetTypeInfo(Main.projFrames[num4], 2, num6);
			randomizedFrameParticle2.Velocity = randomizedFrameParticle.Velocity;
			randomizedFrameParticle2.ColorTint = new Color(255, 255, 255, 0);
			randomizedFrameParticle2.LocalPosition = randomizedFrameParticle.LocalPosition;
			randomizedFrameParticle2.Rotation = randomizedFrameParticle.Rotation;
			randomizedFrameParticle2.Scale = randomizedFrameParticle.Scale * 0.5f;
			randomizedFrameParticle2.FadeInNormalizedTime = randomizedFrameParticle.FadeInNormalizedTime;
			randomizedFrameParticle2.FadeOutNormalizedTime = randomizedFrameParticle.FadeOutNormalizedTime;
			randomizedFrameParticle2.ScaleVelocity = randomizedFrameParticle.ScaleVelocity * 0.5f;
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle2);
		}
	}

	private static void Spawn_StormLightning(ParticleOrchestraSettings settings)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		StormLightningParticle stormLightningParticle = StormLightningParticles.RequestParticle();
		Color white = Color.White;
		if (settings.UniqueInfoPiece != 0)
		{
			white.PackedValue = (uint)settings.UniqueInfoPiece;
		}
		bool flag = settings.IndexOfPlayerWhoInvokedThis != byte.MaxValue;
		int num = 45;
		int seed = (int)settings.MovementVector.X;
		int num2 = (flag ? 12 : 16);
		LightningGenerator generator = (flag ? LightningGenerator.LightningStrikeWeapon : LightningGenerator.StormLightning);
		stormLightningParticle.Prepare(generator, (uint)seed, settings.PositionInWorld, num, white, num2, StormLightningDrawer.DefaultAnim);
		Main.ParticleSystem_World_OverPlayers.Add(stormLightningParticle);
		settings.PositionInWorld = stormLightningParticle.EndPosition;
		if (!flag)
		{
			PunchCameraModifier modifier = new PunchCameraModifier(settings.PositionInWorld, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2() * new Vector2(0.5f, 1f), MathHelper.Lerp(2f, 10f, Main.SceneState.outsideWeatherEffectIntensity), 3f, 45, 1500f, "StormLightning");
			Main.instance.CameraModifiers.Add(modifier);
			Main.NewLightning(instant: true, skipSound: true);
		}
		float volumeScale = MathHelper.Lerp(0.4f, (float)(flag ? 1 : 1), Main.SceneState.outsideWeatherEffectIntensity);
		LegacySoundStyle type = SoundID.InstantThunder;
		if (flag)
		{
			type = SoundID.LightningStrikeHit;
		}
		SoundEngine.PlaySound(type, settings.PositionInWorld, 0f, volumeScale);
		Lighting.AddLight(settings.PositionInWorld, (float)(int)white.R / 255f, (float)(int)white.G / 255f, (float)(int)white.B / 255f);
		int num3 = 3;
		Point val = settings.PositionInWorld.ToTileCoordinates();
		for (int i = val.X - 1; i <= val.X + 1; i++)
		{
			for (int j = val.Y - 1; j <= val.Y + 1; j++)
			{
				Tile tileSafely = Framing.GetTileSafely(i, j);
				if (!tileSafely.active())
				{
					continue;
				}
				int num4 = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely);
				for (int k = 0; k < num4; k++)
				{
					Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
					obj.velocity.Y -= 3f + (float)num3 * 1.5f;
					obj.velocity.Y *= Main.rand.NextFloat();
					obj.velocity.Y *= 0.75f;
					obj.scale += (float)num3 * 0.03f;
				}
				for (int l = 0; l < num4 - 1; l++)
				{
					Dust obj2 = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
					obj2.velocity.Y -= 1f + (float)num3;
					obj2.velocity.Y *= Main.rand.NextFloat();
					obj2.velocity.Y *= 0.75f;
				}
				if (tileSafely.type == 5)
				{
					for (int m = 0; m < 6; m++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6);
						dust.velocity *= 2f;
						dust.fadeIn += Main.rand.NextFloat();
						dust.noLightEmittance = true;
					}
					for (int n = 0; n < 3; n++)
					{
						Dust dust2 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 31);
						dust2.velocity *= 3f;
						dust2.noGravity = true;
						dust2.fadeIn += 1f + Main.rand.NextFloat();
					}
				}
			}
		}
		SpawnLightningExplosionDust(settings.PositionInWorld, white);
		for (int num5 = 0; num5 < 4; num5++)
		{
			Gore gore = Gore.NewGoreDirect(settings.PositionInWorld + Utils.RandomVector2(Main.rand, -15f, 15f) * new Vector2(0.5f, 1f) + new Vector2(-20f, 0f), Vector2.Zero, 61 + Main.rand.Next(3));
			gore.velocity *= (flag ? 1f : 0.5f);
		}
		Vector2 val2 = new Vector2(flag ? 1.6f : 1.1f);
		Vector2 val3 = new Vector2(-0.9f);
		short num6 = 1091;
		Main.instance.LoadProjectile(num6);
		FadingParticle fadingParticle = _poolFading.RequestParticle();
		fadingParticle.SetBasicInfo(TextureAssets.Projectile[num6], null, Vector2.Zero, settings.PositionInWorld);
		fadingParticle.SetTypeInfo(num);
		fadingParticle.ColorTint = white;
		fadingParticle.ColorTint.A = 0;
		fadingParticle.FadeInNormalizedTime = 0.01f;
		fadingParticle.FadeOutNormalizedTime = 0.6f;
		fadingParticle.Scale = val2;
		fadingParticle.ScaleVelocity = val3 / (float)num;
		fadingParticle.ScaleAcceleration = fadingParticle.ScaleVelocity * (-1f / (float)num);
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		FadingParticle fadingParticle2 = _poolFading.RequestParticle();
		fadingParticle2.SetBasicInfo(TextureAssets.Projectile[num6], null, Vector2.Zero, settings.PositionInWorld);
		fadingParticle2.SetTypeInfo(num);
		fadingParticle2.ColorTint = new Color(255, 255, 255);
		fadingParticle2.FadeInNormalizedTime = 0.01f;
		fadingParticle2.FadeOutNormalizedTime = 0.6f;
		fadingParticle2.Scale = val2 * 0.7f;
		fadingParticle2.ScaleVelocity = val3 * 0.7f / (float)num;
		fadingParticle2.ScaleAcceleration = fadingParticle2.ScaleVelocity * (-1f / (float)num);
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
		float num7 = (flag ? 24f : 12f);
		num6 = 916;
		Main.instance.LoadProjectile(num6);
		for (float num8 = 0f; num8 < 1f; num8 += 1f / num7)
		{
			int num9 = Main.rand.Next(14, 22);
			Vector2 val4 = Main.rand.NextVector2Circular(6f, 6f) * 0.7f;
			Color colorTint = white;
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num6], null, Vector2.Zero, val4);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num6], 3, num9);
			randomizedFrameParticle.Velocity = Main.rand.NextVector2Circular(6f, 3f);
			randomizedFrameParticle.ColorTint = colorTint;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + val4;
			randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
			randomizedFrameParticle.Scale = new Vector2(1.5f, 0.75f) * 0.85f;
			randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
			RandomizedFrameParticle randomizedFrameParticle2 = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle2.SetBasicInfo(TextureAssets.Projectile[num6], null, Vector2.Zero, val4);
			randomizedFrameParticle2.SetTypeInfo(Main.projFrames[num6], 3, num9);
			randomizedFrameParticle2.Velocity = randomizedFrameParticle.Velocity;
			randomizedFrameParticle2.ColorTint = new Color(255, 255, 255, 0);
			randomizedFrameParticle2.LocalPosition = randomizedFrameParticle.LocalPosition;
			randomizedFrameParticle2.Rotation = randomizedFrameParticle.Rotation;
			randomizedFrameParticle2.Scale = randomizedFrameParticle.Scale * 0.5f;
			randomizedFrameParticle2.FadeInNormalizedTime = randomizedFrameParticle.FadeInNormalizedTime;
			randomizedFrameParticle2.FadeOutNormalizedTime = randomizedFrameParticle.FadeOutNormalizedTime;
			randomizedFrameParticle2.ScaleVelocity = randomizedFrameParticle.ScaleVelocity * 0.5f;
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle2);
		}
	}

	public static void SpawnLightningExplosionDust(Vector2 position, Color customColor)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 12; i++)
		{
			Dust dust = Dust.NewDustPerfect(position, 226);
			dust.HackFrame(278);
			dust.color = customColor;
			dust.customData = dust.color;
			dust.velocity *= 1f + Main.rand.NextFloat() * 2.5f;
			dust.velocity += new Vector2(0f, -2f);
			dust.fadeIn = 0f;
			dust.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
			dust.velocity.X *= 2f;
			dust.position -= dust.velocity * 3f;
		}
		for (int j = 0; j < 6; j++)
		{
			Dust dust2 = Dust.NewDustPerfect(position, 226);
			dust2.HackFrame(278);
			dust2.color = customColor;
			dust2.customData = dust2.color;
			dust2.velocity *= 1f + Main.rand.NextFloat() * 2.5f;
			dust2.velocity += new Vector2(0f, -2f);
			dust2.fadeIn = 1.7f + (float)Main.rand.Next() * 1f;
			dust2.noGravity = true;
			dust2.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
			dust2.velocity.X *= 2f;
			dust2.velocity *= 0.6f;
			dust2.position -= dust2.velocity * 3f;
		}
		for (int k = 0; k < 6; k++)
		{
			Dust dust3 = Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(20f, 20f), 226);
			dust3.HackFrame(278);
			dust3.color = customColor;
			dust3.customData = dust3.color;
			dust3.velocity = new Vector2(0f, -1f).RotatedByRandom(0.7853981852531433);
			dust3.velocity *= 3f + Main.rand.NextFloat() * 6.5f;
			dust3.fadeIn = 0f;
			dust3.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
			dust3.noGravity = true;
			dust3.position -= dust3.velocity * 2f;
		}
		for (int l = 0; l < 6; l++)
		{
			Dust dust4 = Dust.NewDustPerfect(position, 306, new Vector2(0f, -4f).RotatedByRandom(1.5707963705062866));
			dust4.color = new Color((int)customColor.R, (int)customColor.G, (int)customColor.B, 0);
			dust4.scale = 2.8f;
			dust4.fadeIn = 0f;
			dust4.noGravity = Main.rand.Next(3) != 0;
			Dust dust5 = Dust.CloneDust(dust4);
			dust5.color = new Color(255, 255, 255, 0);
			dust5.scale = 1.9f;
		}
		for (int m = 0; m < 6; m++)
		{
			Dust dust6 = Dust.NewDustPerfect(position, 306, new Vector2(0f, -2f).RotatedByRandom(1.5707963705062866));
			dust6.color = new Color((int)customColor.R, (int)customColor.G, (int)customColor.B, 0);
			dust6.scale = 2.8f;
			dust6.fadeIn = 0f;
			dust6.noGravity = Main.rand.Next(3) != 0;
			Dust dust7 = Dust.CloneDust(dust6);
			dust7.color = new Color(255, 255, 255, 0);
			dust7.scale = 1.9f;
		}
		for (int n = 0; n < 3; n++)
		{
			Dust dust8 = Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(20f, 20f), 43, Main.rand.NextVector2Circular(6f, 6f) * Main.rand.NextFloat(), 26, Color.Lerp(customColor, Color.White, Main.rand.NextFloat()), 1f + Main.rand.NextFloat() * 1.4f);
			dust8.fadeIn = 1.5f;
			if (dust8.velocity.Y > 0f && Main.rand.Next(2) == 0)
			{
				dust8.velocity.Y *= -1f;
			}
			dust8.noGravity = true;
		}
	}

	private static void Spawn_ArcLightning(ParticleOrchestraSettings settings)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		StormLightningParticle stormLightningParticle = StormLightningParticles.RequestParticle();
		Color white = Color.White;
		if (settings.UniqueInfoPiece != 0)
		{
			white.PackedValue = (uint)settings.UniqueInfoPiece;
		}
		bool flag = settings.MovementVector.Y == 1f;
		Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
		int num = 16;
		if (flag)
		{
			num *= 3;
		}
		int seed = (int)settings.MovementVector.X;
		Vector2 playerAnchorPos = StormLightningParticle.GetPlayerAnchorPos(player);
		Vector2 positionInWorld = settings.PositionInWorld;
		stormLightningParticle.Prepare(LightningGenerator.GetArcSurgeWeaponGenerator(playerAnchorPos, positionInWorld), (uint)seed, playerAnchorPos, positionInWorld, num, white, 12f, 0.75f, StormLightningDrawer.ArcSurge, settings.IndexOfPlayerWhoInvokedThis, steadyLight: true);
		Main.ParticleSystem_World_OverPlayers.Add(stormLightningParticle);
		settings.PositionInWorld = stormLightningParticle.EndPosition;
		Lighting.AddLight(settings.PositionInWorld, (float)(int)white.R / 255f, (float)(int)white.G / 255f, (float)(int)white.B / 255f);
		SoundEngine.PlaySound(SoundID.Item15, playerAnchorPos);
		SoundEngine.PlaySound(SoundID.ArcSurge, playerAnchorPos, Main.rand.NextFloatDirection() * 0.2f - 0.05f, 0.6f);
		int num2 = 1;
		short num3 = 916;
		Main.instance.LoadProjectile(num3);
		for (float num4 = 0f; num4 < 1f; num4 += (float)(1 / num2))
		{
			Main.rand.Next(4, 9);
			Vector2 initialLocalPosition = Main.rand.NextVector2Circular(6f, 6f) * 0.7f;
			Color colorTint = white;
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Projectile[num3], TextureAssets.Projectile[num3].Frame(1, 6, 0, Main.rand.Next(6)), Vector2.Zero, initialLocalPosition);
			fadingParticle.SetTypeInfo(num);
			fadingParticle.Velocity = Vector2.Zero;
			fadingParticle.ColorTint = colorTint;
			fadingParticle.LocalPosition = playerAnchorPos - player.MountedCenter;
			fadingParticle.followPlayerIndex = settings.IndexOfPlayerWhoInvokedThis;
			fadingParticle.Rotation = (positionInWorld - playerAnchorPos).ToRotation() + (float)Math.PI / 2f;
			fadingParticle.Scale = new Vector2(1f, 2f) * 1f;
			fadingParticle.FadeInNormalizedTime = 0.1f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.ScaleVelocity = new Vector2(0.025f);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}
		for (float num5 = 0f; num5 < 1f; num5 += (float)(1 / num2))
		{
			int num6 = num + Main.rand.Next(4, 9);
			Vector2 val = Main.rand.NextVector2Circular(6f, 6f) * 0.7f;
			Color colorTint2 = white;
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num3], null, Vector2.Zero, val);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num3], 3, num6);
			randomizedFrameParticle.Velocity = Vector2.Zero;
			randomizedFrameParticle.ColorTint = colorTint2;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + val;
			randomizedFrameParticle.Rotation = (positionInWorld - playerAnchorPos).ToRotation();
			randomizedFrameParticle.Scale = new Vector2(1.5f, 0.75f) * 0.85f;
			randomizedFrameParticle.FadeInNormalizedTime = 0.5f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
			RandomizedFrameParticle randomizedFrameParticle2 = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle2.SetBasicInfo(TextureAssets.Projectile[num3], null, Vector2.Zero, val);
			randomizedFrameParticle2.SetTypeInfo(Main.projFrames[num3], 3, num6);
			randomizedFrameParticle2.Velocity = randomizedFrameParticle.Velocity;
			randomizedFrameParticle2.ColorTint = new Color(255, 255, 255, 0);
			randomizedFrameParticle2.LocalPosition = randomizedFrameParticle.LocalPosition;
			randomizedFrameParticle2.Rotation = randomizedFrameParticle.Rotation;
			randomizedFrameParticle2.Scale = randomizedFrameParticle.Scale * 0.5f;
			randomizedFrameParticle2.FadeInNormalizedTime = randomizedFrameParticle.FadeInNormalizedTime;
			randomizedFrameParticle2.FadeOutNormalizedTime = randomizedFrameParticle.FadeOutNormalizedTime;
			randomizedFrameParticle2.ScaleVelocity = randomizedFrameParticle.ScaleVelocity * 0.5f;
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle2);
		}
	}

	private static void Spawn_NatureFly(ParticleOrchestraSettings settings)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		int num = _natureFlies.CountParticlesInUse();
		if (num <= 50 || !(Main.rand.NextFloat() >= Utils.Remap(num, 50f, 400f, 0.5f, 0f)))
		{
			LittleFlyingCritterParticle littleFlyingCritterParticle = _natureFlies.RequestParticle();
			Color overrideColor = Main.hslToRgb(Main.rand.NextFloat() * 0.14f, 0.1f + 0.6f * Main.rand.NextFloat(), 0.3f + 0.3f * Main.rand.NextFloat());
			overrideColor.A = 170;
			littleFlyingCritterParticle.Prepare(LittleFlyingCritterParticle.FlyType.RegularFly, settings.PositionInWorld, Main.rand.Next(180, 301), overrideColor, 1);
			Main.ParticleSystem_World_OverPlayers.Add(littleFlyingCritterParticle);
		}
	}

	private static void Spawn_LakeSparkle(ParticleOrchestraSettings settings)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		FadingParticle fadingParticle = _poolFading.RequestParticle();
		Vector2 val = Vector2.UnitY * (-0.1f - 0.4f * Main.rand.NextFloat());
		val = settings.MovementVector / 60f;
		fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, Vector2.Zero, settings.PositionInWorld);
		float num = 50f + 90f * Main.rand.NextFloat();
		fadingParticle.SetTypeInfo(num);
		fadingParticle.AccelerationPerFrame = val / num;
		fadingParticle.ColorTint = new Color(255, 255, 255, 0) * Main.rand.NextFloat();
		fadingParticle.FadeInNormalizedTime = 0.45f;
		fadingParticle.FadeOutNormalizedTime = 0.45f;
		fadingParticle.Rotation = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		fadingParticle.RotationVelocity = Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) / 32f;
		fadingParticle.RotationVelocity *= Utils.Clamp(settings.MovementVector.Length() / 32f, 0f, 1f);
		fadingParticle.Scale = Vector2.One * (0.5f + 0.5f * Main.rand.NextFloat());
		Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle);
	}

	private static void Spawn_FakeFish(ParticleOrchestraSettings settings, bool jumping = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		FakeFishParticle fakeFishParticle = _fakeFish.RequestParticle();
		Vector2 movementVector = settings.MovementVector;
		int uniqueInfoPiece = settings.UniqueInfoPiece;
		int lifeTimeTotal = Main.rand.Next(240, 421);
		fakeFishParticle.Position = settings.PositionInWorld;
		fakeFishParticle.Velocity = Vector2.Zero;
		fakeFishParticle.Prepare(uniqueInfoPiece, lifeTimeTotal, movementVector);
		if (jumping)
		{
			fakeFishParticle.Velocity = settings.MovementVector;
			fakeFishParticle.TryJumping();
		}
		Main.ParticleSystem_World_BehindPlayers.Add(fakeFishParticle);
	}

	public static void MagnetFakeFish(Projectile proj, int searchedItemType)
	{
		foreach (FakeFishParticle item in Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>().OrderBy((FakeFishParticle x) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return x.Position.Distance(proj.Center);
		}))
		{
			if (item.TryToMagnetizeTo(proj, searchedItemType))
			{
				return;
			}
		}
		using IEnumerator<FakeFishParticle> enumerator = Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>().OrderBy((FakeFishParticle x) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return x.Position.Distance(proj.Center);
		}).GetEnumerator();
		while (enumerator.MoveNext() && !enumerator.Current.TryToMagnetizeTo(proj))
		{
		}
	}

	public static void PingFakeFish(Projectile proj, int searchedItemType)
	{
		foreach (FakeFishParticle item in Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>().OrderBy((FakeFishParticle x) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return x.Position.Distance(proj.Center);
		}))
		{
			item.TryToBePinged(proj, searchedItemType);
		}
	}

	public static void PushAwayFakeFish(Projectile proj, int searchedItemType)
	{
		foreach (FakeFishParticle item in Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>().OrderBy((FakeFishParticle x) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return x.Position.Distance(proj.Center);
		}))
		{
			item.TryToPushAway(proj, searchedItemType);
		}
	}

	public static void RepelAt(Vector2 position, float radius, bool wet)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		ParticleRepelDetails details = new ParticleRepelDetails
		{
			SourcePosition = position,
			Radius = radius,
			IsInWater = wet
		};
		foreach (IParticle particle in Main.ParticleSystem_World_BehindPlayers.Particles)
		{
			if (particle is IParticleRepel particleRepel)
			{
				particleRepel.BeRepelled(ref details);
			}
		}
		foreach (IParticle particle2 in Main.ParticleSystem_World_OverPlayers.Particles)
		{
			if (particle2 is IParticleRepel particleRepel2)
			{
				particleRepel2.BeRepelled(ref details);
			}
		}
	}

	private static void Spawn_RainbowBoulder4(ParticleOrchestraSettings settings, bool pet = false)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		if (!pet)
		{
			SoundEngine.PlaySound(SoundID.RainbowBoulder, (int)settings.PositionInWorld.X, (int)settings.PositionInWorld.Y);
		}
		int num = 14;
		int num2 = 20;
		if (pet)
		{
			num = 8;
			num2 = 10;
		}
		for (int i = 0; i < num; i++)
		{
			int num3 = Dust.NewDust(settings.PositionInWorld, 0, 0, 66, 0f, 0f, 100, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), pet ? 1.2f : 1.7f);
			Main.dust[num3].noGravity = true;
			if (pet)
			{
				Dust obj = Main.dust[num3];
				obj.velocity *= 2f;
				Main.dust[num3].position = settings.PositionInWorld + Main.rand.NextVector2Circular(30f, 30f);
			}
			else
			{
				Dust obj2 = Main.dust[num3];
				obj2.velocity *= 3f;
				Main.dust[num3].position = settings.PositionInWorld + Main.rand.NextVector2Circular(30f, 30f);
			}
		}
		int num4 = 20;
		if (pet)
		{
			num4 = 12;
		}
		Rectangle rect = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2((float)num4, (float)num4));
		float num5 = settings.MovementVector.ToRotation() + (float)Math.PI / 2f;
		for (float num6 = 0f; num6 < (float)num2; num6++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			int num7 = Main.rand.Next(20, 40);
			prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
			prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
			prettySparkleParticle.Rotation = (float)Math.PI / 2f + num5;
			prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 1f, 0.7f + Main.rand.NextFloat() * 0.7f);
			prettySparkleParticle.Velocity = new Vector2(0f, -1f).RotatedBy(num5);
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num7;
			prettySparkleParticle.FadeOutEnd = num7;
			prettySparkleParticle.FadeInEnd = num7 / 2;
			prettySparkleParticle.FadeOutStart = num7 / 2;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
			prettySparkleParticle2.Rotation = (float)Math.PI / 2f + num5;
			prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
			prettySparkleParticle2.Velocity = new Vector2(0f, 1f).RotatedBy(num5);
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num7;
			prettySparkleParticle2.FadeOutEnd = num7;
			prettySparkleParticle2.FadeInEnd = num7 / 2;
			prettySparkleParticle2.FadeOutStart = num7 / 2;
			prettySparkleParticle2.AdditiveAmount = 1f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
	}

	private static void Spawn_RainbowBoulder1(ParticleOrchestraSettings settings)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 6 + Main.rand.Next(7);
		float num3 = Main.rand.NextFloat();
		Main.rand.Next(2);
		for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
		{
			Vector2 zero = Vector2.Zero;
			Vector2 val = new Vector2(Main.rand.NextFloat() * 0.3f + 1f);
			float num5 = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			num5 = num + (float)Math.PI * 2f * num4;
			float rotation = (float)Math.PI / 2f;
			Vector2 val2 = 0.8f * val * (0.9f + 0.1f * Main.rand.NextFloat());
			float num6 = Main.rand.Next(20) + 180;
			Vector2 val3 = Main.rand.NextVector2Circular(16f, 16f) * val;
			val2 = Main.rand.NextVector2CircularEdge(1.5f, 1.5f);
			val2.X = 0f;
			if (val2.Y > 0f)
			{
				val2.Y *= -1f;
			}
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = val2 + zero;
			prettySparkleParticle.AccelerationPerFrame = num5.ToRotationVector2() * -(val2 / num6) - zero * 1f / num6;
			prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			prettySparkleParticle.ColorTint = Main.hslToRgb((num4 + num3) % 1f, 1f, 0.5f);
			prettySparkleParticle.ColorTint.A = byte.MaxValue;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val3;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = val2 + zero;
			prettySparkleParticle.AccelerationPerFrame = num5.ToRotationVector2() * -(val2 / num6) - zero * 1f / num6;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val3;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val * 0.3f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 21; i++)
		{
			Dust dust = Dust.NewDustDirect(settings.PositionInWorld, 16, 16, 66, 0f, 0f, 100, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), 1.7f);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * (8f + 2f * Main.rand.NextFloat());
			dust.fadeIn = 1f + Main.rand.NextFloat();
			dust.velocity.X = 0f;
			if (dust.velocity.Y > 0f)
			{
				dust.velocity.Y *= -1f;
			}
			dust.position -= dust.velocity * 5f;
		}
	}

	private static void Spawn_RainbowBoulder2(ParticleOrchestraSettings settings)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		FadingParticle fadingParticle = _poolFading.RequestParticle();
		fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, settings.MovementVector, settings.PositionInWorld);
		float num = 25f;
		fadingParticle.SetTypeInfo(num);
		fadingParticle.AccelerationPerFrame = settings.MovementVector / num;
		fadingParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
		fadingParticle.FadeInNormalizedTime = 0.5f;
		fadingParticle.FadeOutNormalizedTime = 0.5f;
		fadingParticle.Rotation = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		fadingParticle.Scale = Vector2.One * (0.5f + 0.5f * Main.rand.NextFloat());
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		FadingParticle fadingParticle2 = fadingParticle;
		fadingParticle = _poolFading.RequestParticle();
		fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, settings.MovementVector, settings.PositionInWorld);
		fadingParticle.SetTypeInfo(num);
		fadingParticle.AccelerationPerFrame = settings.MovementVector / num;
		fadingParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 1f);
		fadingParticle.ColorTint.A = 30;
		fadingParticle.FadeInNormalizedTime = 0.5f;
		fadingParticle.FadeOutNormalizedTime = 0.5f;
		fadingParticle.Rotation = fadingParticle2.Rotation;
		fadingParticle.Scale = fadingParticle2.Scale * 0.5f;
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
	}

	private static void Spawn_RainbowBoulder3(ParticleOrchestraSettings settings)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 3 + Main.rand.Next(7);
		float num3 = Main.rand.NextFloat();
		float num4 = (float)Math.PI / 2f * (float)(Main.rand.Next(2) * 2 - 1);
		for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
		{
			Vector2 val = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
			Vector2 val2 = new Vector2(Main.rand.NextFloat() * 0.2f + 0.3f);
			float num6 = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			num6 = num + (float)Math.PI * 2f * num5;
			float rotation = (float)Math.PI / 2f;
			Vector2 val3 = 0.8f * val2 * (0.3f + 1.5f * Main.rand.NextFloat());
			float num7 = Main.rand.Next(20) + 70;
			Vector2 val4 = ((float)Math.PI * 2f * num5 + num).ToRotationVector2() * (14f - num2 + Main.rand.NextFloat() * 3f);
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = num6.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = num6.ToRotationVector2() * -(val3 / num7) - val * 1f / num7;
			prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			prettySparkleParticle.ColorTint = Main.hslToRgb((num5 + num3) % 1f, 1f, 0.5f);
			prettySparkleParticle.ColorTint.A = byte.MaxValue;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2;
			PrettySparkleParticle prettySparkleParticle2 = prettySparkleParticle;
			prettySparkleParticle2.LocalPosition += prettySparkleParticle.Velocity * 4f;
			prettySparkleParticle.Velocity = prettySparkleParticle.Velocity.RotatedBy(num4);
			prettySparkleParticle.AccelerationPerFrame = prettySparkleParticle.AccelerationPerFrame.RotatedBy(num4);
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = num6.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = num6.ToRotationVector2() * -(val3 / num7) - val * 1f / num7;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2 * 0.3f;
			PrettySparkleParticle prettySparkleParticle3 = prettySparkleParticle;
			prettySparkleParticle3.LocalPosition += prettySparkleParticle.Velocity * 4f;
			prettySparkleParticle.Velocity = prettySparkleParticle.Velocity.RotatedBy(num4);
			prettySparkleParticle.AccelerationPerFrame = prettySparkleParticle.AccelerationPerFrame.RotatedBy(num4);
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 4; i++)
		{
			Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			int num8 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num8].velocity = Main.rand.NextVector2Circular(1f, 1f);
			Dust obj = Main.dust[num8];
			obj.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
			Main.dust[num8].noGravity = true;
			Main.dust[num8].scale = 0.6f + Main.rand.NextFloat() * 0.9f;
			Main.dust[num8].fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
			Main.dust[num8].noLight = true;
			Main.dust[num8].noLightEmittance = true;
			if (num8 != 6000)
			{
				Dust dust = Dust.CloneDust(num8);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
				dust.noLight = true;
				dust.noLightEmittance = true;
			}
		}
	}

	private static void Spawn_MoonLordWhip(ParticleOrchestraSettings settings)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = (float)Math.PI * 2f * Main.rand.NextFloat();
		num2 = settings.MovementVector.SafeNormalize(Vector2.Zero).ToRotation();
		for (float num3 = 0f; num3 < 1f; num3++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 val = num2.ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.7f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = val.ToRotation();
			prettySparkleParticle.Scale = new Vector2(6f, 0.75f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.LocalPosition -= val * num * 0.25f;
			prettySparkleParticle.Velocity = val;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 1f; num4++)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 val2 = num2.ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(0.2f, 1f, 0.4f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = val2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(6f, 0.75f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.LocalPosition -= val2 * num * 0.25f;
			prettySparkleParticle2.Velocity = val2;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 2; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 229, val2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.2f;
				Dust dust2 = dust;
				dust2.position += dust.velocity * 4f;
				dust = Dust.NewDustPerfect(settings.PositionInWorld, 229, -val2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.2f;
				Dust dust3 = dust;
				dust3.position += dust.velocity * 4f;
			}
		}
	}

	private static void Spawn_DeadCellsBarrelExplosion(ParticleOrchestraSettings settings)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> textureAsset = TextureAssets.Extra[269];
		Rectangle value = new Rectangle(0, 0, 100, 100);
		Rectangle value2 = new Rectangle(0, 101, 100, 100);
		Rectangle value3 = new Rectangle(0, 202, 100, 100);
		Color colorTint = new Color(52, 208, 254, 20);
		Color val = new Color(52, 208, 254, 20);
		float num = 25f;
		FadingParticle fadingParticle = _poolFading.RequestParticle();
		fadingParticle.SetBasicInfo(textureAsset, value, Vector2.Zero, settings.PositionInWorld);
		fadingParticle.SetTypeInfo(num);
		fadingParticle.ColorTint = val;
		fadingParticle.Rotation = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		fadingParticle.RotationVelocity = (float)Math.PI * 2f * (1f / num) * 1f * Main.rand.NextFloatDirection();
		fadingParticle.RotationAcceleration = (0f - fadingParticle.RotationVelocity) * (1f / num);
		fadingParticle.FadeInNormalizedTime = 0.01f;
		fadingParticle.FadeOutNormalizedTime = 0.1f;
		fadingParticle.Scale = Vector2.One * 1f;
		fadingParticle.ScaleVelocity = Vector2.One * (2f / num);
		fadingParticle.ScaleAcceleration = fadingParticle.ScaleVelocity * (-1f / num);
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		FadingParticle fadingParticle2 = _poolFading.RequestParticle();
		fadingParticle2.SetBasicInfo(textureAsset, value2, Vector2.Zero, settings.PositionInWorld);
		fadingParticle2.SetTypeInfo(num * 0.8f);
		fadingParticle2.ColorTint = val * 0.25f;
		fadingParticle2.FadeInNormalizedTime = 0.1f;
		fadingParticle2.FadeOutNormalizedTime = 0.1f;
		fadingParticle2.Scale = Vector2.One * 0.5f;
		fadingParticle2.ScaleVelocity = Vector2.One * 8.5f * (1f / num);
		fadingParticle2.ScaleAcceleration = fadingParticle2.ScaleVelocity * (-1f / num);
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
		FadingParticle fadingParticle3 = _poolFading.RequestParticle();
		fadingParticle3.SetBasicInfo(textureAsset, value2, Vector2.Zero, settings.PositionInWorld);
		fadingParticle3.SetTypeInfo(num);
		fadingParticle3.ColorTint = val * 0.3f;
		fadingParticle3.FadeInNormalizedTime = 0.4f;
		fadingParticle3.FadeOutNormalizedTime = 0.5f;
		fadingParticle3.Scale = fadingParticle.Scale;
		fadingParticle3.ScaleVelocity = fadingParticle.ScaleVelocity;
		fadingParticle3.ScaleAcceleration = fadingParticle.ScaleAcceleration;
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle3);
		FadingParticle fadingParticle4 = _poolFading.RequestParticle();
		fadingParticle4.SetBasicInfo(textureAsset, value3, Vector2.Zero, settings.PositionInWorld);
		fadingParticle4.SetTypeInfo(num);
		fadingParticle4.ColorTint = val;
		fadingParticle4.FadeInNormalizedTime = 0.01f;
		fadingParticle4.FadeOutNormalizedTime = 0.7f;
		fadingParticle4.Scale = fadingParticle.Scale * 0.9f;
		fadingParticle4.ScaleVelocity = fadingParticle.ScaleVelocity * 1.5f;
		fadingParticle4.ScaleAcceleration = fadingParticle.ScaleAcceleration * 1.5f;
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle4);
		for (float num2 = 0f; num2 < 1f; num2 += 0.04f)
		{
			float num3 = 25f;
			float num4 = (float)Math.PI * 2f * num2;
			FadingParticle fadingParticle5 = _poolFading.RequestParticle();
			fadingParticle5.SetBasicInfo(TextureAssets.Extra[89], null, num4.ToRotationVector2() * (4f + 7f * Main.rand.NextFloat()), settings.PositionInWorld + num4.ToRotationVector2() * (10f + 90f * Main.rand.NextFloat()));
			fadingParticle5.SetTypeInfo(num3);
			fadingParticle5.AccelerationPerFrame = fadingParticle5.Velocity * (-1f / num3);
			fadingParticle5.LocalPosition -= fadingParticle5.Velocity * 4f;
			fadingParticle5.ColorTint = colorTint;
			fadingParticle5.Rotation = num4 + (float)Math.PI / 2f;
			fadingParticle5.FadeInNormalizedTime = 0.2f;
			fadingParticle5.FadeOutNormalizedTime = 0.3f;
			fadingParticle5.Scale = new Vector2(0.4f, 0.8f) * (0.6f + 0.6f * Main.rand.NextFloat());
			Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle5);
		}
		for (float num5 = 0f; num5 < 1f; num5 += 1f / 7f)
		{
			Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 267, Vector2.Zero, 0, new Color(100, 200, 255, 127), 2f);
			dust.noGravity = true;
			dust.velocity = new Vector2(0f, 12f).RotatedBy((float)Math.PI * 2f * num5) * Main.rand.NextFloat();
		}
	}

	private static void Spawn_DeadCellsFlint(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		float num = settings.MovementVector.Y;
		if (num == 0f)
		{
			num = 1f;
		}
		float x = settings.MovementVector.X;
		bool flag = false;
		Point val = (positionInWorld + new Vector2(0f - x, -160f)).ToTileCoordinates();
		Point val2 = (positionInWorld + new Vector2(x, 160f)).ToTileCoordinates();
		int num2 = 3;
		val.X -= num2;
		val.Y -= num2;
		val2.X += num2;
		val2.Y += num2;
		int num3 = val.X / 2 + val2.X / 2;
		int num4 = (int)x / 2 + 8 * num2;
		float hue = 0.12f;
		float num5 = 2f;
		float num6 = 3f;
		float num7 = 40f;
		int num8 = 3;
		for (int i = val.X; i <= val2.X; i++)
		{
			for (int j = val.Y; j <= val2.Y; j++)
			{
				if (Vector2.Distance(positionInWorld, new Vector2((float)(i * 16), (float)(j * 16))) > (float)num4)
				{
					continue;
				}
				Tile tileSafely = Framing.GetTileSafely(i, j);
				if (!tileSafely.active() || !Main.tileSolid[tileSafely.type] || Main.tileSolidTop[tileSafely.type] || Main.tileFrameImportant[tileSafely.type])
				{
					continue;
				}
				Tile tileSafely2 = Framing.GetTileSafely(i, j - 1);
				if (tileSafely2.active() && Main.tileSolid[tileSafely2.type] && !Main.tileSolidTop[tileSafely2.type])
				{
					continue;
				}
				flag = true;
				if (i <= val.X + num2 && i >= val.X - num2 && j <= val.Y + num2 && j >= val2.Y - num2)
				{
					continue;
				}
				int num9 = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely);
				for (int k = 0; k < num9; k++)
				{
					Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
					obj.velocity.Y -= 3f + (float)num8 * 1.5f;
					obj.velocity.Y *= Main.rand.NextFloat();
					obj.velocity.Y *= 0.75f;
					obj.scale += (float)num8 * 0.03f;
				}
				if (num8 >= 2)
				{
					for (int l = 0; l < num9 - 1; l++)
					{
						Dust obj2 = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
						obj2.velocity.Y -= 1f + (float)num8;
						obj2.velocity.Y *= Main.rand.NextFloat();
						obj2.velocity.Y *= 0.75f;
					}
				}
				if (num9 > 0 && Main.rand.Next(2) != 0)
				{
					float num10 = (float)Math.Abs(num3 - i) / (num7 / 2f);
					Gore gore = Gore.NewGoreDirect(positionInWorld, Vector2.Zero, 61 + Main.rand.Next(3), 1f - (float)num8 * 0.15f + num10 * 0.5f);
					gore.velocity.Y -= 0.1f + (float)num8 * 0.5f + num10 * (float)num8 * 1f;
					gore.velocity.Y *= Main.rand.NextFloat();
					gore.velocity.Y *= num;
					gore.position = new Vector2((float)(i * 16) + 20f * num, (float)(j * 16));
					PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
					int num11 = Main.rand.Next(20, 40);
					prettySparkleParticle.ColorTint = Main.hslToRgb(hue, 1f, 0.5f, 0);
					prettySparkleParticle.LocalPosition = gore.position;
					prettySparkleParticle.Rotation = (float)Math.PI / 2f;
					prettySparkleParticle.Scale = new Vector2(num5 + Main.rand.NextFloat() * num6, 0.7f + Main.rand.NextFloat() * 0.7f);
					prettySparkleParticle.Velocity = new Vector2(0f, -2f * num);
					prettySparkleParticle.FadeInNormalizedTime = 0.1f;
					prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
					prettySparkleParticle.TimeToLive = num11;
					prettySparkleParticle.FadeOutEnd = num11;
					prettySparkleParticle.FadeInEnd = num11 / 2;
					prettySparkleParticle.FadeOutStart = num11 / 2;
					prettySparkleParticle.AdditiveAmount = 0.35f;
					prettySparkleParticle.DrawVerticalAxis = false;
					Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				}
			}
		}
		if (!flag)
		{
			return;
		}
		SoundEngine.PlaySound(SoundID.DeadCellsFlintRelease, positionInWorld);
		for (int m = 0; m < 30; m++)
		{
			Vector2 val3 = Main.rand.NextVector2Circular(5f, 3f);
			if (val3.Y > 0f)
			{
				val3.Y *= -1f;
			}
			Dust.NewDustPerfect(positionInWorld + Main.rand.NextVector2Circular(7f, 0f), 228, val3, 0, default, 0.7f + 0.8f * Main.rand.NextFloat());
		}
		SoundEngine.PlayTrackedSound(SoundID.DD2_MonkStaffGroundImpact, positionInWorld);
	}

	private static void Spawn_MoonLordWhipEye(ParticleOrchestraSettings settings)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = TextureAssets.Extra[270];
		if (val.IsLoaded)
		{
			float num = 1f;
			float num2 = 45f;
			float f = (float)Math.PI * 2f * Main.rand.NextFloat();
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num2);
			fadingParticle.ColorTint = new Color(255, 255, 255, 0);
			fadingParticle.LocalPosition = settings.PositionInWorld + f.ToRotationVector2() * num * num2 * 0.5f;
			fadingParticle.Rotation = 0f;
			fadingParticle.FadeInNormalizedTime = 0.01f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.Velocity = settings.MovementVector * 0.05f;
			fadingParticle.AccelerationPerFrame = -(settings.MovementVector * 0.25f) / num2;
			fadingParticle.Scale = new Vector2(1f, 1f) * (0.7f + 0.45f * Main.rand.NextFloat());
			Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle);
		}
	}

	private static void Spawn_DeadCellsHeadEffect(ParticleOrchestraSettings settings)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = TextureAssets.Extra[266];
		if (!val.IsLoaded)
		{
			return;
		}
		Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
		if (player.active)
		{
			Vector2 movementVector = settings.MovementVector;
			movementVector += new Vector2(0f, 0f);
			for (float num = 0f; num < 1f; num += 0.5f)
			{
				float num2 = 0.5f * Utils.Remap(settings.MovementVector.Length(), 0f, 10f, 0.5f, 1f);
				float num3 = 1f;
				float num4 = 15f;
				float f = (float)Math.PI * 2f * Main.rand.NextFloat();
				Vector2 val2 = Main.rand.NextVector2Circular(4f, 4f) * num2;
				FadingPlayerShaderParticle fadingPlayerShaderParticle = _poolFadingPlayerShader.RequestParticle();
				fadingPlayerShaderParticle.SetBasicInfo(val, null, Vector2.Zero, Vector2.Zero);
				fadingPlayerShaderParticle.SetTypeInfo(num4, player, player.cHead, fullbright: false);
				fadingPlayerShaderParticle.Velocity = movementVector * 0.5f;
				fadingPlayerShaderParticle.AccelerationPerFrame = fadingPlayerShaderParticle.Velocity * (0f - -1f / num4) * 0.25f;
				fadingPlayerShaderParticle.LocalPosition = settings.PositionInWorld + f.ToRotationVector2() * num3 * num2 * num4 * 0.5f + val2 + movementVector * num;
				fadingPlayerShaderParticle.LocalPosition += fadingPlayerShaderParticle.Velocity * -0.5f;
				fadingPlayerShaderParticle.Rotation = movementVector.ToRotation() + (float)Math.PI / 2f;
				fadingPlayerShaderParticle.FadeInNormalizedTime = 0.01f;
				fadingPlayerShaderParticle.FadeOutNormalizedTime = 0.1f;
				fadingPlayerShaderParticle.Scale = new Vector2(0.6f, 1.2f) * num2;
				fadingPlayerShaderParticle.ScaleVelocity = fadingPlayerShaderParticle.Scale * (-1f / num4);
				Main.ParticleSystem_World_BehindPlayers.Add(fadingPlayerShaderParticle);
			}
		}
	}

	private static void Spawn_UFOLaser(ParticleOrchestraSettings settings)
	{
		SpawnHelper_SpawnInLine(settings, (Vector2 dustPos, Vector2 velocity) =>
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			SpawnHelper_SpawnSingleLineDust(160, dustPos, (Main.rand.Next(2) == 0) ? Color.LimeGreen : Color.CornflowerBlue);
			SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.25f, (Main.rand.Next(2) == 0) ? Color.LimeGreen : Color.CornflowerBlue);
		});
	}

	private static void Spawn_MagnetSphereBolt(ParticleOrchestraSettings settings)
	{
		SpawnHelper_SpawnInLine(settings, (Vector2 dustPos, Vector2 velocity) =>
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			SpawnHelper_SpawnSingleLineDust(160, dustPos);
			SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.25f);
			SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.5f);
			SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.75f);
		});
	}

	private static void Spawn_HeatRay(ParticleOrchestraSettings settings)
	{
		SpawnHelper_SpawnInLine(settings, (Vector2 dustPos, Vector2 velocity) =>
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			SpawnHelper_SpawnSingleLineDust(162, dustPos);
			SpawnHelper_SpawnSingleLineDust(162, dustPos - velocity * 0.25f);
			SpawnHelper_SpawnSingleLineDust(162, dustPos - velocity * 0.5f);
			SpawnHelper_SpawnSingleLineDust(162, dustPos - velocity * 0.75f);
		});
	}

	private static void Spawn_Shadowbeam(ParticleOrchestraSettings settings, bool hostile = true)
	{
		float velocityScalar = 0.25f;
		if (hostile)
		{
			velocityScalar = 0.03334f;
		}
		SpawnHelper_SpawnInLine(settings, (Vector2 dustPos, Vector2 velocity) =>
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			SpawnHelper_SpawnSingleLineDust(173, dustPos);
			SpawnHelper_SpawnSingleLineDust(173, dustPos - velocity * velocityScalar);
			SpawnHelper_SpawnSingleLineDust(173, dustPos - velocity * velocityScalar * 2f);
		});
	}

	private static void SpawnHelper_SpawnInLine(ParticleOrchestraSettings settings, Action<Vector2, Vector2> effectsPerStep = null)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		if (effectsPerStep != null)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			Vector2 movementVector = settings.MovementVector;
			Vector2 arg = (positionInWorld - movementVector).SafeNormalize(Vector2.Zero) * ((float)settings.UniqueInfoPiece / 1000f);
			Vector2 val = positionInWorld - movementVector;
			float num = val.Length();
			float num2 = arg.Length();
			if (num2 < 4f)
			{
				num2 = 4f;
			}
			for (float num3 = 0f; num3 < num; num3 += num2)
			{
				float num4 = num3 / num;
				Vector2 arg2 = Vector2.Lerp(movementVector, positionInWorld, num4);
				effectsPerStep(arg2, arg);
			}
		}
	}

	private static void SpawnHelper_SpawnSingleLineDust(int dustId, Vector2 dustPos, Color color = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = Main.dust[Dust.NewDust(dustPos, 1, 1, dustId)];
		dust.position = dustPos;
		dust.scale = (float)Main.rand.Next(70, 110) * 0.013f;
		dust.velocity *= 0.2f;
		if (color != default(Color))
		{
			dust.color = color;
		}
	}

	private static void Spawn_ShadowOrbExplosion(ParticleOrchestraSettings settings)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		float num = 20f + 10f * Main.rand.NextFloat();
		float num2 = -(float)Math.PI / 4f;
		float num3 = 0.2f + 0.4f * Main.rand.NextFloat();
		Color val = Main.hslToRgb(Main.rand.NextFloat() * 0.05f + 0.75f, 1f, 0.5f);
		val.A /= 2;
		val *= Main.rand.NextFloat() * 0.3f + 0.7f;
		for (float num4 = 0f; num4 < 2f; num4++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 val2 = ((float)Math.PI / 4f + (float)Math.PI * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = val;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + Main.rand.NextVector2Circular(16f, 16f);
			prettySparkleParticle.Rotation = val2.ToRotation();
			prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1.1f * num3;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.LocalPosition -= val2 * num * 0.25f;
			prettySparkleParticle.Velocity = val2 * 0.05f;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num4 == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
				prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num5 = 0f; num5 < 2f; num5++)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 val3 = ((float)Math.PI / 4f + (float)Math.PI * num5 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(91f / 120f, 0.4f, 0.2f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld + Main.rand.NextVector2Circular(16f, 16f);
			prettySparkleParticle2.Rotation = val3.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(4f, 1f) * 0.7f * num3;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.LocalPosition -= val3 * num * 0.25f;
			prettySparkleParticle2.Velocity = val3 * 0.05f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num5 == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
				prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 1; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 6, val3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.4f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 6, -val3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
		Vector2 position = settings.PositionInWorld + new Vector2(-18f, -16f);
		float circleHalfWidth = 4f;
		float circleHalfHeight = 4f;
		for (int j = 0; j < 0; j++)
		{
			_ = j % 2;
			Gore gore = Gore.NewGoreDirect(position, Vector2.Zero, 99, 1f + Main.rand.NextFloat() * 0.3f);
			gore.velocity = Main.rand.NextVector2Circular(circleHalfWidth, circleHalfHeight);
			gore.alpha = 127;
			gore.rotation = (float)Math.PI * 2f * Main.rand.NextFloat();
			Gore gore2 = gore;
			gore2.position += gore.velocity * 4f;
			gore = Gore.NewGoreDirect(position, Vector2.Zero, 99, 0.8f + Main.rand.NextFloat() * 0.3f);
			gore.velocity = Main.rand.NextVector2Circular(circleHalfWidth, circleHalfHeight);
			gore.alpha = 127;
			gore.rotation = (float)Math.PI * 2f * Main.rand.NextFloat();
			Gore gore3 = gore;
			gore3.position += gore.velocity * 4f;
			gore = Gore.NewGoreDirect(position, Vector2.Zero, 99, 1.2f + Main.rand.NextFloat() * 0.3f);
			gore.velocity = Main.rand.NextVector2Circular(circleHalfWidth, circleHalfHeight);
			gore.alpha = 127;
			gore.rotation = (float)Math.PI * 2f * Main.rand.NextFloat();
			Gore gore4 = gore;
			gore4.position += gore.velocity * 4f;
		}
	}

	private static void Spawn_DeadCellsDownDashExplosion(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		SoundEngine.PlayTrackedSound(SoundID.DD2_MonkStaffGroundImpact, positionInWorld);
		float x = settings.MovementVector.X;
		Point val = (positionInWorld + new Vector2(0f - x, -160f)).ToTileCoordinates();
		Point val2 = (positionInWorld + new Vector2(x, 160f)).ToTileCoordinates();
		int num = val.X / 2 + val2.X / 2;
		int num2 = (int)x / 2;
		bool flag = settings.UniqueInfoPiece == 1;
		float num3 = (flag ? 0.01f : 0.55f);
		float num4 = (flag ? 2f : 1f);
		float num5 = (flag ? 3f : 2f);
		for (int i = 0; i < 40; i++)
		{
			Vector2 val3 = Main.rand.NextVector2Circular(5f, 3f);
			if (val3.Y > 0f)
			{
				val3.Y *= -1f;
			}
			if (flag)
			{
				val3 *= 1.5f;
			}
			Dust dust = Dust.CloneDust(Dust.NewDustPerfect(positionInWorld + Main.rand.NextVector2Circular(10f, 0f), 306, val3, 0, Main.hslToRgb(num3, 1f, 0.5f), 1.9f + 0.8f * Main.rand.NextFloat()));
			dust.scale -= 0.6f;
			dust.color = Color.White;
		}
		float num6 = 40f;
		int num7 = 3;
		for (int j = val.X; j <= val2.X; j++)
		{
			for (int k = val.Y; k <= val2.Y; k++)
			{
				if (Vector2.Distance(positionInWorld, new Vector2((float)(j * 16), (float)(k * 16))) > (float)num2)
				{
					continue;
				}
				Tile tileSafely = Framing.GetTileSafely(j, k);
				if (!tileSafely.active() || !Main.tileSolid[tileSafely.type] || (Main.tileSolidTop[tileSafely.type] && tileSafely.frameY != 0) || (Main.tileFrameImportant[tileSafely.type] && !Main.tileSolidTop[tileSafely.type]))
				{
					continue;
				}
				Tile tileSafely2 = Framing.GetTileSafely(j, k - 1);
				if (tileSafely2.active() && Main.tileSolid[tileSafely2.type] && !Main.tileSolidTop[tileSafely2.type])
				{
					continue;
				}
				int num8 = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely);
				for (int l = 0; l < num8; l++)
				{
					Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
					obj.velocity.Y -= 3f + (float)num7 * 1.5f;
					obj.velocity.Y *= Main.rand.NextFloat();
					obj.velocity.Y *= 0.75f;
					obj.scale += (float)num7 * 0.03f;
				}
				if (num7 >= 2)
				{
					for (int m = 0; m < num8 - 1; m++)
					{
						Dust obj2 = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
						obj2.velocity.Y -= 1f + (float)num7;
						obj2.velocity.Y *= Main.rand.NextFloat();
						obj2.velocity.Y *= 0.75f;
					}
				}
				if (num8 > 0 && Main.rand.Next(3) != 0)
				{
					float num9 = (float)Math.Abs(num - j) / (num6 / 2f);
					Gore gore = Gore.NewGoreDirect(positionInWorld, Vector2.Zero, 61 + Main.rand.Next(3), 1f - (float)num7 * 0.15f + num9 * 0.5f);
					gore.velocity.Y -= 0.1f + (float)num7 * 0.5f + num9 * (float)num7 * 1f;
					gore.velocity.Y *= Main.rand.NextFloat();
					gore.position = new Vector2((float)(j * 16 + 20), (float)(k * 16));
					PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
					int num10 = Main.rand.Next(20, 40);
					prettySparkleParticle.ColorTint = Main.hslToRgb(num3 + 0.1f * Main.rand.NextFloat(), 1f, 0.5f, 0);
					prettySparkleParticle.LocalPosition = gore.position;
					prettySparkleParticle.Rotation = (float)Math.PI / 2f;
					prettySparkleParticle.Scale = new Vector2(num4 + Main.rand.NextFloat() * num5, 0.7f + Main.rand.NextFloat() * 0.7f);
					prettySparkleParticle.Velocity = new Vector2(0f, -2f);
					prettySparkleParticle.FadeInNormalizedTime = 0.1f;
					prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
					prettySparkleParticle.TimeToLive = num10;
					prettySparkleParticle.FadeOutEnd = num10;
					prettySparkleParticle.FadeInEnd = num10 / 2;
					prettySparkleParticle.FadeOutStart = num10 / 2;
					prettySparkleParticle.AdditiveAmount = 0.35f;
					prettySparkleParticle.DrawVerticalAxis = false;
					Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				}
			}
		}
	}

	private static void Spawn_ShimmerTownNPCSend(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rect = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2(30f, 60f));
		for (float num = 0f; num < 20f; num++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			int num2 = Main.rand.Next(20, 40);
			prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
			prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
			prettySparkleParticle.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 2f, 0.7f + Main.rand.NextFloat() * 0.7f);
			prettySparkleParticle.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num2;
			prettySparkleParticle.FadeOutEnd = num2;
			prettySparkleParticle.FadeInEnd = num2 / 2;
			prettySparkleParticle.FadeOutStart = num2 / 2;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
			prettySparkleParticle2.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
			prettySparkleParticle2.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num2;
			prettySparkleParticle2.FadeOutEnd = num2;
			prettySparkleParticle2.FadeInEnd = num2 / 2;
			prettySparkleParticle2.FadeOutStart = num2 / 2;
			prettySparkleParticle2.AdditiveAmount = 1f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
	}

	private static void Spawn_ShimmerTownNPC(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2(30f, 60f));
		for (float num = 0f; num < 20f; num++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			int num2 = Main.rand.Next(20, 40);
			prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
			prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(val);
			prettySparkleParticle.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 2f, 0.7f + Main.rand.NextFloat() * 0.7f);
			prettySparkleParticle.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num2;
			prettySparkleParticle.FadeOutEnd = num2;
			prettySparkleParticle.FadeInEnd = num2 / 2;
			prettySparkleParticle.FadeOutStart = num2 / 2;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(val);
			prettySparkleParticle2.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
			prettySparkleParticle2.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num2;
			prettySparkleParticle2.FadeOutEnd = num2;
			prettySparkleParticle2.FadeInEnd = num2 / 2;
			prettySparkleParticle2.FadeOutStart = num2 / 2;
			prettySparkleParticle2.AdditiveAmount = 1f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
		for (int i = 0; i < 20; i++)
		{
			int num3 = Dust.NewDust(val.TopLeft(), val.Width, val.Height, 308);
			Main.dust[num3].velocity.Y -= 8f;
			Main.dust[num3].velocity.X *= 0.5f;
			Main.dust[num3].scale = 0.8f;
			Main.dust[num3].noGravity = true;
			switch (Main.rand.Next(6))
			{
			case 0:
				Main.dust[num3].color = new Color(255, 255, 210);
				break;
			case 1:
				Main.dust[num3].color = new Color(190, 245, 255);
				break;
			case 2:
				Main.dust[num3].color = new Color(255, 150, 255);
				break;
			default:
				Main.dust[num3].color = new Color(190, 175, 255);
				break;
			}
		}
		SoundEngine.PlaySound(SoundID.Item29, settings.PositionInWorld);
	}

	private static void Spawn_PooFly(ParticleOrchestraSettings settings)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		int num = _poolFlies.CountParticlesInUse();
		if (num <= 50 || !(Main.rand.NextFloat() >= Utils.Remap(num, 50f, 400f, 0.5f, 0f)))
		{
			LittleFlyingCritterParticle littleFlyingCritterParticle = _poolFlies.RequestParticle();
			littleFlyingCritterParticle.Prepare(LittleFlyingCritterParticle.FlyType.RegularFly, settings.PositionInWorld, 300);
			Main.ParticleSystem_World_OverPlayers.Add(littleFlyingCritterParticle);
		}
	}

	private static void Spawn_Digestion(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		int num = ((settings.MovementVector.X < 0f) ? 1 : (-1));
		int num2 = Main.rand.Next(4);
		for (int i = 0; i < 3 + num2; i++)
		{
			int num3 = Dust.NewDust(positionInWorld + Vector2.UnitX * (float)(-num) * 8f - Vector2.One * 5f + Vector2.UnitY * 8f, 3, 6, 216, -num, 1f);
			Dust obj = Main.dust[num3];
			obj.velocity /= 2f;
			Main.dust[num3].scale = 0.8f;
		}
		if (Main.rand.Next(30) == 0)
		{
			int num4 = Gore.NewGore(positionInWorld + Vector2.UnitX * (float)(-num) * 8f, Vector2.Zero, Main.rand.Next(580, 583));
			Gore obj2 = Main.gore[num4];
			obj2.velocity /= 2f;
			Main.gore[num4].velocity.Y = Math.Abs(Main.gore[num4].velocity.Y);
			Main.gore[num4].velocity.X = (0f - Math.Abs(Main.gore[num4].velocity.X)) * (float)num;
		}
		SoundEngine.PlaySound(SoundID.Item16, settings.PositionInWorld);
	}

	private static void Spawn_ShimmerBlock(ParticleOrchestraSettings settings)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)settings.UniqueInfoPiece / 1000f;
		if (num <= 0f)
		{
			num = 1f;
		}
		FadingParticle fadingParticle = _poolFading.RequestParticle();
		fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, settings.MovementVector, settings.PositionInWorld);
		float num2 = 45f;
		fadingParticle.SetTypeInfo(num2);
		fadingParticle.AccelerationPerFrame = settings.MovementVector / num2;
		fadingParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 0.75f, 0.8f);
		fadingParticle.ColorTint.A = 30;
		fadingParticle.FadeInNormalizedTime = 0.5f;
		fadingParticle.FadeOutNormalizedTime = 0.5f;
		fadingParticle.Rotation = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		fadingParticle.Scale = Vector2.One * (0.5f + 0.5f * Main.rand.NextFloat()) * num;
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
	}

	private static void Spawn_LoadOutChange(ParticleOrchestraSettings settings)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
		if (player.active)
		{
			Rectangle hitbox = player.Hitbox;
			int num = 6;
			hitbox.Height -= num;
			if (player.gravDir == 1f)
			{
				hitbox.Y += num;
			}
			for (int i = 0; i < 40; i++)
			{
				Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox), 16, null, 120, default, Main.rand.NextFloat() * 0.8f + 0.8f);
				dust.velocity = new Vector2(0f, (float)(-hitbox.Height) * Main.rand.NextFloat() * 0.04f).RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.1f);
				dust.velocity += player.velocity * 2f * Main.rand.NextFloat();
				dust.noGravity = true;
				dust.noLight = (dust.noLightEmittance = true);
			}
			for (int j = 0; j < 5; j++)
			{
				Dust dust2 = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox), 43, null, 254, Main.hslToRgb(Main.rand.NextFloat(), 0.3f, 0.8f), Main.rand.NextFloat() * 0.8f + 0.8f);
				dust2.velocity = new Vector2(0f, (float)(-hitbox.Height) * Main.rand.NextFloat() * 0.04f).RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.1f);
				dust2.velocity += player.velocity * 2f * Main.rand.NextFloat();
				dust2.noGravity = true;
				dust2.noLight = (dust2.noLightEmittance = true);
			}
		}
	}

	private static void Spawn_TownSlimeTransform(ParticleOrchestraSettings settings)
	{
		switch (settings.UniqueInfoPiece)
		{
		case 0:
			NerdySlimeEffect(settings);
			break;
		case 1:
			CopperSlimeEffect(settings);
			break;
		case 2:
			ElderSlimeEffect(settings);
			break;
		}
	}

	private static void Spawn_DeadCellsMushroomBoiExplosion(ParticleOrchestraSettings settings)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		if (Main.rand.Next(2) == 0)
		{
			int num = Main.rand.Next(_mushBoiExplosionSounds.Length);
			ActiveSound activeSound = SoundEngine.GetActiveSound(_mushBoiExplosionSounds[num]);
			if (activeSound != null && activeSound.IsPlaying)
			{
				activeSound.Stop();
			}
			_mushBoiExplosionSounds[num] = SoundEngine.PlayTrackedSound(SoundID.DeadCellsMushroomExplode, settings.PositionInWorld);
		}
		BloodyExplosionParticle bloodyExplosionParticle = _poolBloodyExplosion.RequestParticle();
		Color lightColorTint = new Color(255, 10, 10, 50) * 0.35f;
		bloodyExplosionParticle.ColorTint = (bloodyExplosionParticle.LightColorTint = lightColorTint);
		bloodyExplosionParticle.LocalPosition = settings.PositionInWorld;
		bloodyExplosionParticle.TimeToLive = 10f;
		bloodyExplosionParticle.FadeInNormalizedTime = 0.3f;
		bloodyExplosionParticle.FadeOutNormalizedTime = 0.6f;
		bloodyExplosionParticle.Velocity = Vector2.Zero;
		bloodyExplosionParticle.InitialScale = 1f;
		Main.ParticleSystem_World_BehindPlayers.Add(bloodyExplosionParticle);
		Asset<Texture2D> textureAsset = TextureAssets.Extra[269];
		Rectangle value = new Rectangle(0, 0, 100, 100);
		Rectangle value2 = new Rectangle(0, 101, 100, 100);
		Rectangle value3 = new Rectangle(0, 202, 100, 100);
		Color colorTint = new Color(255, 10, 10, 127) * 0.45f;
		Color val = new Color(255, 10, 10, 127) * 0.45f;
		float num2 = 15f;
		FadingParticle fadingParticle = _poolFading.RequestParticle();
		fadingParticle.SetBasicInfo(textureAsset, value, Vector2.Zero, settings.PositionInWorld);
		fadingParticle.SetTypeInfo(num2);
		fadingParticle.ColorTint = val;
		fadingParticle.Rotation = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		fadingParticle.RotationVelocity = (float)Math.PI * 2f * (1f / num2) * 1f * Main.rand.NextFloatDirection();
		fadingParticle.RotationAcceleration = (0f - fadingParticle.RotationVelocity) * (1f / num2);
		fadingParticle.FadeInNormalizedTime = 0.01f;
		fadingParticle.FadeOutNormalizedTime = 0.1f;
		fadingParticle.Scale = Vector2.One * 0.6f;
		fadingParticle.ScaleVelocity = Vector2.One * (1.2f / num2);
		fadingParticle.ScaleAcceleration = fadingParticle.ScaleVelocity * (-1f / num2);
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		FadingParticle fadingParticle2 = _poolFading.RequestParticle();
		fadingParticle2.SetBasicInfo(textureAsset, value2, Vector2.Zero, settings.PositionInWorld);
		fadingParticle2.SetTypeInfo(num2 * 0.8f);
		fadingParticle2.ColorTint = val * 0.25f;
		fadingParticle2.FadeInNormalizedTime = 0.1f;
		fadingParticle2.FadeOutNormalizedTime = 0.1f;
		fadingParticle2.Scale = Vector2.One * 0.3f;
		fadingParticle2.ScaleVelocity = Vector2.One * 6.5f * (1f / num2);
		fadingParticle2.ScaleAcceleration = fadingParticle2.ScaleVelocity * (-1f / num2);
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
		FadingParticle fadingParticle3 = _poolFading.RequestParticle();
		fadingParticle3.SetBasicInfo(textureAsset, value2, Vector2.Zero, settings.PositionInWorld);
		fadingParticle3.SetTypeInfo(num2);
		fadingParticle3.ColorTint = val * 0.3f;
		fadingParticle3.FadeInNormalizedTime = 0.4f;
		fadingParticle3.FadeOutNormalizedTime = 0.5f;
		fadingParticle3.Scale = fadingParticle.Scale;
		fadingParticle3.ScaleVelocity = fadingParticle.ScaleVelocity;
		fadingParticle3.ScaleAcceleration = fadingParticle.ScaleAcceleration;
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle3);
		FadingParticle fadingParticle4 = _poolFading.RequestParticle();
		fadingParticle4.SetBasicInfo(textureAsset, value3, Vector2.Zero, settings.PositionInWorld);
		fadingParticle4.SetTypeInfo(num2);
		fadingParticle4.ColorTint = val;
		fadingParticle4.FadeInNormalizedTime = 0.01f;
		fadingParticle4.FadeOutNormalizedTime = 0.7f;
		fadingParticle4.Scale = fadingParticle.Scale * 0.9f;
		fadingParticle4.ScaleVelocity = fadingParticle.ScaleVelocity * 1.5f;
		fadingParticle4.ScaleAcceleration = fadingParticle.ScaleAcceleration * 1.5f;
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle4);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / 6f)
		{
			float num4 = 15f;
			float num5 = (float)Math.PI * 2f * num3;
			FadingParticle fadingParticle5 = _poolFading.RequestParticle();
			fadingParticle5.SetBasicInfo(TextureAssets.Extra[89], null, num5.ToRotationVector2() * (4f + 7f * Main.rand.NextFloat()), settings.PositionInWorld + num5.ToRotationVector2() * (10f + 90f * Main.rand.NextFloat()));
			fadingParticle5.SetTypeInfo(num4);
			fadingParticle5.AccelerationPerFrame = fadingParticle5.Velocity * (-1f / num4);
			fadingParticle5.LocalPosition -= fadingParticle5.Velocity * 4f;
			fadingParticle5.ColorTint = colorTint;
			fadingParticle5.Rotation = num5 + (float)Math.PI / 2f;
			fadingParticle5.FadeInNormalizedTime = 0.2f;
			fadingParticle5.FadeOutNormalizedTime = 0.3f;
			fadingParticle5.Scale = new Vector2(0.4f, 0.8f) * (0.6f + 0.6f * Main.rand.NextFloat());
			Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle5);
		}
	}

	private static void Spawn_DeadCellsMushroomBoiTargetFound(ParticleOrchestraSettings settings)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		ShockIconParticle shockIconParticle = _poolShockIcon.RequestParticle();
		shockIconParticle.ColorTint = new Color(255, 255, 10, 150);
		shockIconParticle.LocalPosition = settings.PositionInWorld;
		shockIconParticle.TimeToLive = 15f;
		shockIconParticle.FadeInNormalizedTime = 0.2f;
		shockIconParticle.FadeOutNormalizedTime = 0.7f;
		shockIconParticle.Velocity = Vector2.Zero;
		shockIconParticle.InitialScale = 1f;
		shockIconParticle.ParentProjectileKey = (ProjectileKey)settings.UniqueInfoPiece;
		shockIconParticle.OffsetFromParent = new Vector2(0f, -40f);
		Main.ParticleSystem_World_BehindPlayers.Add(shockIconParticle);
	}

	private static void Spawn_DeadCellsBarnacleShotFiring(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		Vector2 movementVector = settings.MovementVector;
		SoundEngine.PlaySound(SoundID.Item95, positionInWorld);
		for (int i = 0; i < 15; i++)
		{
			Vector2 position = positionInWorld + new Vector2((float)Main.rand.Next(-4, 4), (float)Main.rand.Next(-4, 4));
			Vector2 val = movementVector.SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * ((i >= 8) ? (3f + Main.rand.NextFloat() * 2f) : (7f + Main.rand.NextFloat() * 2f));
			Dust dust = Dust.NewDustPerfect(position, 2, val);
			dust.alpha = 50;
			dust.scale = 1.2f + (float)Main.rand.Next(-5, 5) * 0.01f;
			dust.fadeIn = 0.5f;
			dust.noGravity = true;
			dust.velocity = val;
		}
		for (int j = 0; j < 5; j++)
		{
			Vector2 position2 = positionInWorld + new Vector2((float)Main.rand.Next(-4, 4), (float)Main.rand.Next(-4, 4));
			Vector2 val2 = movementVector.SafeNormalize(Vector2.Zero).RotatedByRandom(0.09817477315664291) * (0f + Main.rand.NextFloat() * 8f);
			Dust dust2 = Dust.NewDustPerfect(position2, 267, val2);
			dust2.alpha = 127;
			dust2.color = Color.Lerp(Color.DarkOliveGreen, Color.White, Main.rand.NextFloat() * 0.25f);
			dust2.fadeIn = 0.6f + Main.rand.NextFloat() * 0.6f;
			dust2.scale = 0.5f;
			dust2.noGravity = true;
			dust2.velocity = val2;
		}
	}

	private static void ElderSlimeEffect(ParticleOrchestraSettings settings)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 30; i++)
		{
			Dust dust = Dust.NewDustPerfect(settings.PositionInWorld + Main.rand.NextVector2Circular(20f, 20f), 43, (settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat(), 26, Color.Lerp(Main.OurFavoriteColor, Color.White, Main.rand.NextFloat()), 1f + Main.rand.NextFloat() * 1.4f);
			dust.fadeIn = 1.5f;
			if (dust.velocity.Y > 0f && Main.rand.Next(2) == 0)
			{
				dust.velocity.Y *= -1f;
			}
			dust.noGravity = true;
		}
		for (int j = 0; j < 8; j++)
		{
			Gore gore = Gore.NewGoreDirect(settings.PositionInWorld + Utils.RandomVector2(Main.rand, -30f, 30f) * new Vector2(0.5f, 1f), Vector2.Zero, 61 + Main.rand.Next(3));
			gore.velocity *= 0.5f;
		}
	}

	private static void NerdySlimeEffect(ParticleOrchestraSettings settings)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Color newColor = new Color(0, 80, 255, 100);
		for (int i = 0; i < 60; i++)
		{
			Dust.NewDustPerfect(settings.PositionInWorld, 4, (settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat(), 175, newColor, 0.6f + Main.rand.NextFloat() * 1.4f);
		}
	}

	private static void CopperSlimeEffect(ParticleOrchestraSettings settings)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 40; i++)
		{
			Dust dust = Dust.NewDustPerfect(settings.PositionInWorld + Main.rand.NextVector2Circular(20f, 20f), 43, (settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat(), 26, Color.Lerp(new Color(183, 88, 25), Color.White, Main.rand.NextFloat() * 0.5f), 1f + Main.rand.NextFloat() * 1.4f);
			dust.fadeIn = 1.5f;
			if (dust.velocity.Y > 0f && Main.rand.Next(2) == 0)
			{
				dust.velocity.Y *= -1f;
			}
			dust.noGravity = true;
		}
	}

	private static void Spawn_ShimmerArrow(ParticleOrchestraSettings settings)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		float num = 20f;
		for (int i = 0; i < 2; i++)
		{
			float num2 = (float)Math.PI * 2f * Main.rand.NextFloatDirection() * 0.05f;
			Color val = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
			val.A /= 2;
			Color val2 = val;
			val2.A = byte.MaxValue;
			val2 = Color.Lerp(val2, Color.White, 0.5f);
			for (float num3 = 0f; num3 < 4f; num3++)
			{
				PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
				Vector2 val3 = ((float)Math.PI / 2f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = val;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = val3.ToRotation();
				prettySparkleParticle.Scale = new Vector2((num3 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.Velocity = -val3 * 0.2f;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num3 % 2f == 1f)
				{
					prettySparkleParticle.Scale *= 0.9f;
					prettySparkleParticle.Velocity *= 0.9f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = 0f; num4 < 4f; num4++)
			{
				PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
				Vector2 val4 = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = val2;
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = val4.ToRotation();
				prettySparkleParticle2.Scale = new Vector2((num4 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.Velocity = val4 * 0.2f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				if (num4 % 2f == 1f)
				{
					prettySparkleParticle2.Scale *= 1.2f;
					prettySparkleParticle2.Velocity *= 1.2f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				if (i == 0)
				{
					for (int j = 0; j < 1; j++)
					{
						Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 306, val4.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
						dust.noGravity = true;
						dust.scale = 1.4f;
						dust.fadeIn = 1.2f;
						dust.color = val;
						Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 306, -val4.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
						dust2.noGravity = true;
						dust2.scale = 1.4f;
						dust2.fadeIn = 1.2f;
						dust2.color = val;
					}
				}
			}
		}
	}

	private static void Spawn_ItemTransfer(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		Vector2 val = settings.PositionInWorld + settings.MovementVector;
		int key = settings.UniqueInfoPiece & 0xFFFFFF;
		BitsByte bitsByte = (byte)(settings.UniqueInfoPiece >> 24);
		bool flag = bitsByte[0];
		bool flag2 = bitsByte[1];
		bool transitionIn = bitsByte[2];
		bool fullbright = bitsByte[3];
		if (ContentSamples.ItemsByType.TryGetValue(key, out var value) && !value.IsAir)
		{
			key = value.type;
			int num = Main.rand.Next(60, 80);
			Chest.AskForChestToEatItem(val + new Vector2(-8f, -8f), num + 10);
			Vector2 val2 = Main.rand.NextVector2Square(-1f, 1f);
			ItemTransferParticle itemTransferParticle = _poolItemTransfer.RequestParticle();
			itemTransferParticle.Prepare(key, num, positionInWorld, val, flag ? (val2 * 24f) : Vector2.Zero, flag2 ? (val2 * 8f) : Vector2.Zero, transitionIn, fullbright, inInventory: false);
			Main.ParticleSystem_World_OverPlayers.Add(itemTransferParticle);
		}
	}

	private static void Spawn_PetExchange(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		for (int i = 0; i < 13; i++)
		{
			Gore gore = Gore.NewGoreDirect(positionInWorld + new Vector2(-20f, -20f) + Main.rand.NextVector2Circular(20f, 20f), Vector2.Zero, Main.rand.Next(61, 64), 1f + Main.rand.NextFloat() * 0.3f);
			gore.alpha = 100;
			gore.velocity = ((float)Math.PI * 2f * (float)Main.rand.Next()).ToRotationVector2() * Main.rand.NextFloat() + settings.MovementVector * 0.5f;
		}
	}

	private static void Spawn_TerraBlade(ParticleOrchestraSettings settings)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = settings.MovementVector.ToRotation() + (float)Math.PI / 2f;
		float num3 = 3f;
		for (float num4 = 0f; num4 < 4f; num4++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 val = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.2f, 0.85f, 0.4f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = val.ToRotation();
			prettySparkleParticle.Scale = new Vector2(num3, 0.5f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.Velocity = -val * 0.2f;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num4 % 2f == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 2f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num5 = -1f; num5 <= 1f; num5 += 2f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			_ = num2.ToRotationVector2() * 4f;
			Vector2 val2 = ((float)Math.PI / 2f * num5 + num2).ToRotationVector2() * 2f;
			prettySparkleParticle2.ColorTint = new Color(0.4f, 1f, 0.4f, 0.5f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = val2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(num3, 0.5f) * 1.1f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.AdditiveAmount = 0.35f;
			prettySparkleParticle2.Velocity = val2.RotatedBy(1.5707963705062866) * 0.5f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
		for (float num6 = 0f; num6 < 4f; num6++)
		{
			PrettySparkleParticle prettySparkleParticle3 = _poolPrettySparkle.RequestParticle();
			Vector2 val3 = ((float)Math.PI / 2f * num6 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle3.ColorTint = new Color(0.2f, 1f, 0.2f, 1f);
			prettySparkleParticle3.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle3.Rotation = val3.ToRotation();
			prettySparkleParticle3.Scale = new Vector2(num3, 0.5f) * 0.7f;
			prettySparkleParticle3.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle3.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle3.TimeToLive = num;
			prettySparkleParticle3.FadeOutEnd = num;
			prettySparkleParticle3.FadeInEnd = num / 2f;
			prettySparkleParticle3.FadeOutStart = num / 2f;
			prettySparkleParticle3.Velocity = val3 * 0.2f;
			prettySparkleParticle3.DrawVerticalAxis = false;
			if (num6 % 2f == 1f)
			{
				prettySparkleParticle3.Scale *= 1.5f;
				prettySparkleParticle3.Velocity *= 2f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle3);
			for (int i = 0; i < 1; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 107, val3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 0.8f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 107, -val3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_Excalibur(ParticleOrchestraSettings settings)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = 0f;
		for (float num3 = 0f; num3 < 4f; num3++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 val = ((float)Math.PI / 2f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.9f, 0.85f, 0.4f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = val.ToRotation();
			prettySparkleParticle.Scale = new Vector2((num3 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.Velocity = -val * 0.2f;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num3 % 2f == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 4f; num4++)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 val2 = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(1f, 1f, 0.2f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = val2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2((num4 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.Velocity = val2 * 0.2f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num4 % 2f == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 1; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 169, val2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.4f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 169, -val2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_SlapHand(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(SoundID.Item175, settings.PositionInWorld);
	}

	private static void Spawn_WaffleIron(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(SoundID.Item178, settings.PositionInWorld);
	}

	private static void Spawn_PlayerVoiceOverrideSound(ParticleOrchestraSettings settings)
	{
		byte indexOfPlayerWhoInvokedThis = settings.IndexOfPlayerWhoInvokedThis;
		if (indexOfPlayerWhoInvokedThis >= 0 && indexOfPlayerWhoInvokedThis < byte.MaxValue)
		{
			Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
			if (player.active && !player.dead)
			{
				sbyte voiceOverride = (sbyte)settings.UniqueInfoPiece;
				sbyte voiceOverride2 = player.voiceOverride;
				player.voiceOverride = voiceOverride;
				player.PlayHurtSound();
				player.voiceOverride = voiceOverride2;
			}
		}
	}

	private static void Spawn_ClassyCane(ParticleOrchestraSettings settings)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		int num = 7;
		float scale = 1.1f;
		int type = 10;
		Color newColor = default;
		Vector2 positionInWorld = settings.PositionInWorld;
		int num2 = 20;
		int num3 = 20;
		positionInWorld.X -= num2 / 2;
		positionInWorld.Y -= num3 / 2;
		int num4 = Main.rand.Next(0, 2);
		for (int i = 0; i < num4; i++)
		{
			int num5 = Gore.NewGore(new Vector2(positionInWorld.X + (float)Main.rand.Next(num2), positionInWorld.Y + (float)Main.rand.Next(num3)), Vector2.Zero, 1218);
			Main.gore[num5].velocity = new Vector2((float)Main.rand.Next(-10, 11) * 0.4f, 0f - (3f + (float)Main.rand.Next(6) * 0.3f));
		}
		for (int j = 0; j < num; j++)
		{
			int num6 = Dust.NewDust(positionInWorld, num2, num3, type, 0f, -1f, 80, newColor, scale);
			if (Main.rand.Next(3) != 0)
			{
				Main.dust[num6].noGravity = true;
			}
		}
	}

	private static void Spawn_FlyMeal(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(SoundID.Item16, settings.PositionInWorld);
	}

	private static void Spawn_VampireOnFire(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(SoundID.Item20, settings.PositionInWorld);
	}

	private static void Spawn_GasTrap(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(SoundID.Item16, settings.PositionInWorld);
		Vector2 movementVector = settings.MovementVector;
		int num = 12;
		int num2 = 10;
		float num3 = 5f;
		float num4 = 2.5f;
		Color lightColorTint = new Color(0.2f, 0.4f, 0.15f);
		Vector2 positionInWorld = settings.PositionInWorld;
		float num5 = (float)Math.PI / 20f;
		float num6 = (float)Math.PI / 15f;
		for (int i = 0; i < num; i++)
		{
			Vector2 spinninpoint = movementVector + new Vector2(num3 + Main.rand.NextFloat() * 1f, 0f).RotatedBy((float)i / (float)num * ((float)Math.PI * 2f), Vector2.Zero);
			spinninpoint = spinninpoint.RotatedByRandom(num5);
			GasParticle gasParticle = _poolGas.RequestParticle();
			gasParticle.AccelerationPerFrame = Vector2.Zero;
			gasParticle.Velocity = spinninpoint;
			gasParticle.ColorTint = Color.White;
			gasParticle.LightColorTint = lightColorTint;
			gasParticle.LocalPosition = positionInWorld + spinninpoint;
			gasParticle.TimeToLive = 50 + Main.rand.Next(20);
			gasParticle.InitialScale = 1f + Main.rand.NextFloat() * 0.35f;
			Main.ParticleSystem_World_BehindPlayers.Add(gasParticle);
		}
		for (int j = 0; j < num2; j++)
		{
			Vector2 spinninpoint2 = new Vector2(num4 + Main.rand.NextFloat() * 1.45f, 0f).RotatedBy((float)j / (float)num2 * ((float)Math.PI * 2f), Vector2.Zero);
			spinninpoint2 = spinninpoint2.RotatedByRandom(num6);
			if (j % 2 == 0)
			{
				spinninpoint2 *= 0.5f;
			}
			GasParticle gasParticle2 = _poolGas.RequestParticle();
			gasParticle2.AccelerationPerFrame = Vector2.Zero;
			gasParticle2.Velocity = spinninpoint2;
			gasParticle2.ColorTint = Color.White;
			gasParticle2.LightColorTint = lightColorTint;
			gasParticle2.LocalPosition = positionInWorld;
			gasParticle2.TimeToLive = 80 + Main.rand.Next(30);
			gasParticle2.InitialScale = 1f + Main.rand.NextFloat() * 0.5f;
			Main.ParticleSystem_World_BehindPlayers.Add(gasParticle2);
		}
	}

	private static void Spawn_TrueExcalibur(ParticleOrchestraSettings settings)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		float num = 36f;
		float num2 = (float)Math.PI / 4f;
		for (float num3 = 0f; num3 < 2f; num3++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 v = ((float)Math.PI / 2f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(1f, 0f, 0.3f, 1f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = v.ToRotation();
			prettySparkleParticle.Scale = new Vector2(5f, 0.5f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 2f; num4++)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 val = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(1f, 0.5f, 0.8f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = val.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(3f, 0.5f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 1; i++)
			{
				if (Main.rand.Next(2) != 0)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 242, val.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust.noGravity = true;
					dust.scale = 1.4f;
					Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 242, -val.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust2.noGravity = true;
					dust2.scale = 1.4f;
				}
			}
		}
		num = 30f;
		num2 = 0f;
		for (float num5 = 0f; num5 < 4f; num5++)
		{
			PrettySparkleParticle prettySparkleParticle3 = _poolPrettySparkle.RequestParticle();
			Vector2 val2 = ((float)Math.PI / 2f * num5 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle3.ColorTint = new Color(0.9f, 0.85f, 0.4f, 0.5f);
			prettySparkleParticle3.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle3.Rotation = val2.ToRotation();
			prettySparkleParticle3.Scale = new Vector2((num5 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
			prettySparkleParticle3.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle3.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle3.TimeToLive = num;
			prettySparkleParticle3.FadeOutEnd = num;
			prettySparkleParticle3.FadeInEnd = num / 2f;
			prettySparkleParticle3.FadeOutStart = num / 2f;
			prettySparkleParticle3.AdditiveAmount = 0.35f;
			prettySparkleParticle3.Velocity = -val2 * 0.2f;
			prettySparkleParticle3.DrawVerticalAxis = false;
			if (num5 % 2f == 1f)
			{
				prettySparkleParticle3.Scale *= 1.5f;
				prettySparkleParticle3.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle3);
		}
		for (float num6 = 0f; num6 < 4f; num6++)
		{
			PrettySparkleParticle prettySparkleParticle4 = _poolPrettySparkle.RequestParticle();
			Vector2 val3 = ((float)Math.PI / 2f * num6 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle4.ColorTint = new Color(1f, 1f, 0.2f, 1f);
			prettySparkleParticle4.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle4.Rotation = val3.ToRotation();
			prettySparkleParticle4.Scale = new Vector2((num6 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
			prettySparkleParticle4.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle4.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle4.TimeToLive = num;
			prettySparkleParticle4.FadeOutEnd = num;
			prettySparkleParticle4.FadeInEnd = num / 2f;
			prettySparkleParticle4.FadeOutStart = num / 2f;
			prettySparkleParticle4.Velocity = val3 * 0.2f;
			prettySparkleParticle4.DrawVerticalAxis = false;
			if (num6 % 2f == 1f)
			{
				prettySparkleParticle4.Scale *= 1.5f;
				prettySparkleParticle4.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle4);
			for (int j = 0; j < 1; j++)
			{
				if (Main.rand.Next(2) != 0)
				{
					Dust dust3 = Dust.NewDustPerfect(settings.PositionInWorld, 169, val3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust3.noGravity = true;
					dust3.scale = 1.4f;
					Dust dust4 = Dust.NewDustPerfect(settings.PositionInWorld, 169, -val3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust4.noGravity = true;
					dust4.scale = 1.4f;
				}
			}
		}
	}

	private static void Spawn_BestReforge(ParticleOrchestraSettings settings)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
		Asset<Texture2D> textureAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", (AssetRequestMode)1);
		for (int i = 0; i < 8; i++)
		{
			Vector2 val = Main.rand.NextVector2Circular(3f, 4f);
			if (val.Y > 0f)
			{
				val.Y = 0f - val.Y;
			}
			val.Y -= 2f;
			Main.ParticleSystem_World_OverPlayers.Add(new CreativeSacrificeParticle(textureAsset, null, settings.MovementVector + val, settings.PositionInWorld)
			{
				AccelerationPerFrame = accelerationPerFrame,
				ScaleOffsetPerFrame = -1f / 60f
			});
		}
	}

	private static void Spawn_LeafCrystalPassive(ParticleOrchestraSettings settings)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		float num = 90f;
		float num2 = (float)Math.PI * 2f * Main.rand.NextFloat();
		float num3 = 3f;
		for (float num4 = 0f; num4 < num3; num4++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 v = ((float)Math.PI * 2f / num3 * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.3f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = v.ToRotation();
			prettySparkleParticle.Scale = new Vector2(4f, 1f) * 0.4f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = 10f;
			prettySparkleParticle.FadeOutStart = 10f;
			prettySparkleParticle.AdditiveAmount = 0.5f;
			prettySparkleParticle.Velocity = Vector2.Zero;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
	}

	private static void Spawn_LeafCrystalShot(ParticleOrchestraSettings settings)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		int num = 30;
		PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
		Vector2 movementVector = settings.MovementVector;
		Color val = Main.hslToRgb((float)settings.UniqueInfoPiece / 255f, 1f, 0.5f);
		val = (prettySparkleParticle.ColorTint = Color.Lerp(val, Color.Gold, (float)(int)val.R / 255f * 0.5f));
		prettySparkleParticle.LocalPosition = settings.PositionInWorld;
		prettySparkleParticle.Rotation = movementVector.ToRotation();
		prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1f;
		prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
		prettySparkleParticle.FadeOutNormalizedTime = 1f;
		prettySparkleParticle.TimeToLive = num;
		prettySparkleParticle.FadeOutEnd = num;
		prettySparkleParticle.FadeInEnd = num / 2;
		prettySparkleParticle.FadeOutStart = num / 2;
		prettySparkleParticle.AdditiveAmount = 0.5f;
		prettySparkleParticle.Velocity = settings.MovementVector;
		prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
		prettySparkleParticle.DrawVerticalAxis = false;
		Lighting.AddLight(settings.PositionInWorld, new Vector3(0.05f, 0.2f, 0.1f) * 1.5f);
		Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
	}

	private static void Spawn_TrueNightsEdge(ParticleOrchestraSettings settings)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = 0f;
		for (float num3 = 0f; num3 < 3f; num3 += 2f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 val = ((float)Math.PI / 4f + (float)Math.PI / 4f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.3f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = val.ToRotation();
			prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.LocalPosition -= val * num * 0.25f;
			prettySparkleParticle.Velocity = val;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num3 == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
				prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 3f; num4 += 2f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 val2 = ((float)Math.PI / 4f + (float)Math.PI / 4f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(0.6f, 1f, 0.2f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = val2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(4f, 1f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.LocalPosition -= val2 * num * 0.25f;
			prettySparkleParticle2.Velocity = val2;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num4 == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
				prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 2; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 75, val2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.4f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 75, -val2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_NightsEdge(ParticleOrchestraSettings settings)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = 0f;
		for (float num3 = 0f; num3 < 3f; num3++)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 val = ((float)Math.PI / 4f + (float)Math.PI / 4f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.25f, 0.1f, 0.5f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = val.ToRotation();
			prettySparkleParticle.Scale = new Vector2(2f, 1f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.LocalPosition -= val * num * 0.25f;
			prettySparkleParticle.Velocity = val;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num3 == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
				prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 3f; num4++)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 val2 = ((float)Math.PI / 4f + (float)Math.PI / 4f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(0.5f, 0.25f, 1f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = val2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(2f, 1f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.LocalPosition -= val2 * num * 0.25f;
			prettySparkleParticle2.Velocity = val2;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num4 == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
				prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
	}

	private static void Spawn_SilverBulletSparkle(ParticleOrchestraSettings settings)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		_ = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		Vector2 movementVector = settings.MovementVector;
		Vector2 val = new Vector2(Main.rand.NextFloat() * 0.2f + 0.4f);
		Main.rand.NextFloat();
		float rotation = (float)Math.PI / 2f;
		Vector2 val2 = Main.rand.NextVector2Circular(4f, 4f) * val;
		PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
		prettySparkleParticle.AccelerationPerFrame = -movementVector * 1f / 30f;
		prettySparkleParticle.Velocity = movementVector;
		prettySparkleParticle.ColorTint = Color.White;
		prettySparkleParticle.LocalPosition = settings.PositionInWorld + val2;
		prettySparkleParticle.Rotation = rotation;
		prettySparkleParticle.Scale = val;
		prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
		prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
		prettySparkleParticle.FadeInEnd = 10f;
		prettySparkleParticle.FadeOutStart = 20f;
		prettySparkleParticle.FadeOutEnd = 30f;
		prettySparkleParticle.TimeToLive = 30f;
		Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
	}

	private static void Spawn_PaladinsHammer(ParticleOrchestraSettings settings)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 1f;
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			float num4 = 0.6f + Main.rand.NextFloat() * 0.35f;
			Vector2 val = settings.MovementVector * num4;
			Vector2 val2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.2f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			_ = 0.1f * val2;
			Vector2 val3 = Main.rand.NextVector2Circular(12f, 12f) * val2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.AccelerationPerFrame = -val * 1f / 30f;
			prettySparkleParticle.Velocity = val + f.ToRotationVector2() * 2f * num4;
			prettySparkleParticle.ColorTint = new Color(1f, 0.8f, 0.4f, 0f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val3;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = 40f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.AccelerationPerFrame = -val * 1f / 30f;
			prettySparkleParticle.Velocity = val * 0.8f + f.ToRotationVector2() * 2f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val3;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2 * 0.6f;
			prettySparkleParticle.FadeInNormalizedTime = 0.1f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.9f;
			prettySparkleParticle.TimeToLive = 60f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 2; i++)
		{
			int num5 = Dust.NewDust(newColor: new Color(1f, 0.7f, 0.3f, 0f), Position: settings.PositionInWorld, Width: 0, Height: 0, Type: 267);
			Main.dust[num5].velocity = Main.rand.NextVector2Circular(2f, 2f);
			Dust obj = Main.dust[num5];
			obj.velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
			Main.dust[num5].noGravity = true;
			Main.dust[num5].scale = 0.1f;
			Dust obj2 = Main.dust[num5];
			obj2.position += Main.rand.NextVector2Circular(16f, 16f);
			Main.dust[num5].velocity = settings.MovementVector;
			if (num5 != 6000)
			{
				Dust dust = Dust.CloneDust(num5);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_PrincessWeapon(ParticleOrchestraSettings settings)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 1f;
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			Vector2 val = settings.MovementVector * (0.6f + Main.rand.NextFloat() * 0.35f);
			Vector2 val2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.2f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 val3 = 0.1f * val2;
			float num4 = 60f;
			Vector2 val4 = Main.rand.NextVector2Circular(8f, 8f) * val2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num4) - val * 1f / 30f;
			prettySparkleParticle.AccelerationPerFrame = -val * 1f / 60f;
			prettySparkleParticle.Velocity = val * 0.66f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((0.92f + Main.rand.NextFloat() * 0.02f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			prettySparkleParticle.ColorTint.A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num4) - val * 1f / 15f;
			prettySparkleParticle.AccelerationPerFrame = -val * 1f / 60f;
			prettySparkleParticle.Velocity = val * 0.66f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2 * 0.6f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 2; i++)
		{
			Color newColor = Main.hslToRgb((0.92f + Main.rand.NextFloat() * 0.02f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			int num5 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num5].velocity = Main.rand.NextVector2Circular(2f, 2f);
			Dust obj = Main.dust[num5];
			obj.velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
			Main.dust[num5].noGravity = true;
			Main.dust[num5].scale = 0.1f;
			Dust obj2 = Main.dust[num5];
			obj2.position += Main.rand.NextVector2Circular(16f, 16f);
			Main.dust[num5].velocity = settings.MovementVector;
			if (num5 != 6000)
			{
				Dust dust = Dust.CloneDust(num5);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_StardustPunch(ParticleOrchestraSettings settings)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 1f;
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			Vector2 val = settings.MovementVector * (0.3f + Main.rand.NextFloat() * 0.35f);
			Vector2 val2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 val3 = 0.1f * val2;
			float num4 = 60f;
			Vector2 val4 = Main.rand.NextVector2Circular(8f, 8f) * val2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num4) - val * 1f / 60f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((0.6f + Main.rand.NextFloat() * 0.05f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			prettySparkleParticle.ColorTint.A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num4) - val * 1f / 30f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2 * 0.6f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 2; i++)
		{
			Color newColor = Main.hslToRgb((0.59f + Main.rand.NextFloat() * 0.05f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			int num5 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num5].velocity = Main.rand.NextVector2Circular(2f, 2f);
			Dust obj = Main.dust[num5];
			obj.velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
			Main.dust[num5].noGravity = true;
			Main.dust[num5].scale = 0.6f + Main.rand.NextFloat() * 2f;
			Dust obj2 = Main.dust[num5];
			obj2.position += Main.rand.NextVector2Circular(16f, 16f);
			if (num5 != 6000)
			{
				Dust dust = Dust.CloneDust(num5);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_RainbowRodHit(ParticleOrchestraSettings settings)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 6f;
		float num3 = Main.rand.NextFloat();
		for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
		{
			Vector2 val = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
			Vector2 val2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 val3 = 1.5f * val2;
			float num5 = 60f;
			Vector2 val4 = Main.rand.NextVector2Circular(8f, 8f) * val2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num5) - val * 1f / 60f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			prettySparkleParticle.ColorTint.A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num5) - val * 1f / 60f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2 * 0.6f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 12; i++)
		{
			Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			int num6 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num6].velocity = Main.rand.NextVector2Circular(1f, 1f);
			Dust obj = Main.dust[num6];
			obj.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
			Main.dust[num6].noGravity = true;
			Main.dust[num6].scale = 0.6f + Main.rand.NextFloat() * 0.9f;
			Main.dust[num6].fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
			if (num6 != 6000)
			{
				Dust dust = Dust.CloneDust(num6);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_BlackLightningSmall(ParticleOrchestraSettings settings)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = Main.rand.Next(1, 3);
		float num3 = 0.7f;
		int num4 = 916;
		Main.instance.LoadProjectile(num4);
		Color val = new Color(255, 255, 255, 255);
		Color indigo = Color.Indigo;
		indigo.A = 0;
		for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
		{
			float f = (float)Math.PI * 2f * num5 + num + Main.rand.NextFloatDirection() * 0.25f;
			float num6 = Main.rand.NextFloat() * 4f + 0.1f;
			Vector2 val2 = Main.rand.NextVector2Circular(12f, 12f) * num3;
			Color.Lerp(Color.Lerp(Color.Black, indigo, Main.rand.NextFloat() * 0.5f), val, Main.rand.NextFloat() * 0.6f);
			Color colorTint = new Color(0, 0, 0, 255);
			int num7 = Main.rand.Next(4);
			if (num7 == 1)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 127), Color.Black, 0.1f + 0.7f * Main.rand.NextFloat());
			}
			if (num7 == 2)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 60), Color.Black, 0.1f + 0.8f * Main.rand.NextFloat());
			}
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num4], null, Vector2.Zero, val2);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num4], 2, 24f);
			randomizedFrameParticle.Velocity = f.ToRotationVector2() * num6 * new Vector2(1f, 0.5f) * 0.2f + settings.MovementVector;
			randomizedFrameParticle.ColorTint = colorTint;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + val2;
			randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
			randomizedFrameParticle.Scale = Vector2.One * 0.5f;
			randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
		}
	}

	private static void Spawn_BlackLightningHit(ParticleOrchestraSettings settings)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 7f;
		float num3 = 0.7f;
		int num4 = 916;
		Main.instance.LoadProjectile(num4);
		Color val = new Color(255, 255, 255, 255);
		Color indigo = Color.Indigo;
		indigo.A = 0;
		for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
		{
			float num6 = (float)Math.PI * 2f * num5 + num + Main.rand.NextFloatDirection() * 0.25f;
			float num7 = Main.rand.NextFloat() * 4f + 0.1f;
			Vector2 val2 = Main.rand.NextVector2Circular(12f, 12f) * num3;
			Color.Lerp(Color.Lerp(Color.Black, indigo, Main.rand.NextFloat() * 0.5f), val, Main.rand.NextFloat() * 0.6f);
			Color colorTint = new Color(0, 0, 0, 255);
			int num8 = Main.rand.Next(4);
			if (num8 == 1)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 127), Color.Black, 0.1f + 0.7f * Main.rand.NextFloat());
			}
			if (num8 == 2)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 60), Color.Black, 0.1f + 0.8f * Main.rand.NextFloat());
			}
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num4], null, Vector2.Zero, val2);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num4], 2, 24f);
			randomizedFrameParticle.Velocity = num6.ToRotationVector2() * num7 * new Vector2(1f, 0.5f);
			randomizedFrameParticle.ColorTint = colorTint;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + val2;
			randomizedFrameParticle.Rotation = num6;
			randomizedFrameParticle.Scale = Vector2.One;
			randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.05f);
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
		}
	}

	private static void Spawn_BlueLightningSmallLong(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = settings.PositionInWorld;
		Vector2 val2 = settings.MovementVector.SafeNormalize(Vector2.Zero);
		float num = settings.MovementVector.Length();
		int num2 = 25;
		int num3 = 40;
		if (!(num < (float)num3))
		{
			float num4 = 0f;
			while (num4 < num)
			{
				Spawn_LightningSmall(new ParticleOrchestraSettings
				{
					PositionInWorld = val,
					MovementVector = val2 * 2f,
					UniqueInfoPiece = settings.UniqueInfoPiece
				});
				num4 += (float)num2;
				val += val2 * (float)num2;
			}
		}
	}

	private static void Spawn_LightningSmall(ParticleOrchestraSettings settings)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = Main.rand.Next(1, 3);
		Main.rand.Next(1, 3);
		float num3 = 0.7f;
		short num4 = 916;
		Main.instance.LoadProjectile(num4);
		Color val = new Color(133, 255, 255, 0);
		Color val2 = new Color(15, 100, 155, 0);
		if (settings.UniqueInfoPiece != 0)
		{
			val.PackedValue = (uint)settings.UniqueInfoPiece;
			val.A = 0;
			val2 = val * 0.75f;
		}
		Lighting.AddLight(settings.PositionInWorld, new Vector3(0.4f, 0.8f, 1f) * 0.7f);
		for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
		{
			float f = (float)Math.PI * 2f * num5 + num + Main.rand.NextFloatDirection() * 0.25f;
			float num6 = Main.rand.NextFloat() * 4f + 0.1f;
			Vector2 val3 = Main.rand.NextVector2Circular(6f, 6f) * num3;
			Color val4 = Color.Lerp(val, val2, Main.rand.NextFloat());
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num4], null, Vector2.Zero, val3);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num4], 2, 10f);
			randomizedFrameParticle.Velocity = f.ToRotationVector2() * num6 * new Vector2(1f, 0.5f) * 0.2f + settings.MovementVector;
			randomizedFrameParticle.ColorTint = val4;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + val3;
			randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
			randomizedFrameParticle.Scale = Vector2.One * 1f;
			randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
			if (Main.rand.Next(2) == 0)
			{
				short type = 226;
				if (settings.UniqueInfoPiece != 0)
				{
					type = 331;
				}
				Dust dust = Dust.NewDustPerfect(randomizedFrameParticle.LocalPosition, type, randomizedFrameParticle.Velocity);
				dust.scale = 0.8f;
				dust.noGravity = true;
				dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
				dust.scale *= 1.25f;
				dust.noLight = true;
				if (settings.UniqueInfoPiece != 0)
				{
					dust.color = val4;
				}
			}
		}
	}

	private static void Spawn_StellarTune(ParticleOrchestraSettings settings)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 5f;
		Vector2 val = new Vector2(0.7f);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			float num4 = (float)Math.PI * 2f * num3 + num + Main.rand.NextFloatDirection() * 0.25f;
			Vector2 val2 = 1.5f * val;
			float num5 = 60f;
			Vector2 val3 = Main.rand.NextVector2Circular(12f, 12f) * val;
			Color colorTint = Color.Lerp(Color.Gold, Color.HotPink, Main.rand.NextFloat());
			if (Main.rand.Next(2) == 0)
			{
				colorTint = Color.Lerp(Color.Violet, Color.HotPink, Main.rand.NextFloat());
			}
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = num4.ToRotationVector2() * val2;
			prettySparkleParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(val2 / num5);
			prettySparkleParticle.ColorTint = colorTint;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val3;
			prettySparkleParticle.Rotation = num4;
			prettySparkleParticle.Scale = val * (Main.rand.NextFloat() * 0.8f + 0.2f);
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		num2 = 1f;
	}

	private static void Spawn_FormatC(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		float num = settings.MovementVector.ToRotation() + (float)Math.PI / 2f + 0.9424779f * Main.rand.NextFloatDirection();
		float num2 = 1f;
		Vector2 val = new Vector2(0.7f);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			float num4 = (float)Math.PI * 2f * num3 + num + Main.rand.NextFloatDirection() * 0.1f;
			Vector2 val2 = 3f * val * Main.rand.NextFloatDirection();
			float num5 = 15f;
			Color gold = Color.Gold;
			gold.A = 127;
			Vector2 val3 = Main.rand.NextVector2Circular(8f, 8f) * val;
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Extra[98], null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num5);
			fadingParticle.Velocity = num4.ToRotationVector2() * val2;
			fadingParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(val2 / num5);
			fadingParticle.ColorTint = gold;
			fadingParticle.LocalPosition = settings.PositionInWorld + num4.ToRotationVector2() * -val2 * val * num5 * 0.2f + val3;
			fadingParticle.Rotation = num4 + (float)Math.PI / 2f;
			fadingParticle.FadeInNormalizedTime = 0.3f;
			fadingParticle.FadeOutNormalizedTime = 0.4f;
			fadingParticle.Scale = new Vector2(0.5f, 1.2f) * 0.8f * val;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
			fadingParticle = _poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Extra[98], null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num5);
			fadingParticle.Velocity = num4.ToRotationVector2() * val2;
			fadingParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(val2 / num5);
			fadingParticle.ColorTint = new Color(255, 255, 255, 0);
			fadingParticle.LocalPosition = settings.PositionInWorld + num4.ToRotationVector2() * -val2 * val * num5 * 0.2f + val3;
			fadingParticle.Rotation = num4 + (float)Math.PI / 2f;
			fadingParticle.FadeInNormalizedTime = 0.3f;
			fadingParticle.FadeOutNormalizedTime = 0.4f;
			fadingParticle.Scale = new Vector2(0.5f, 1.2f) * 0.48f * val;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}
		for (int i = 0; i < 12; i++)
		{
			int num6 = Dust.NewDust(settings.PositionInWorld, 1, 1, 213, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
			Main.dust[num6].noGravity = true;
			Main.dust[num6].fadeIn = Main.dust[num6].scale + 1f + 0.01f * (float)Main.rand.Next(0, 51);
			Main.dust[num6].noGravity = true;
			Dust obj = Main.dust[num6];
			obj.velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num6].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
			Main.dust[num6].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num6].position.Y -= 4f;
			Main.dust[num6].velocity = Main.dust[num6].velocity.RotatedBy(num + ((Main.dust[num6].velocity.X > 0f) ? ((float)Math.PI / 2f) : (-(float)Math.PI / 2f)));
			Dust obj2 = Main.dust[num6];
			obj2.position -= Main.dust[num6].velocity * 5f;
			if (Main.rand.Next(3) != 0)
			{
				Main.dust[num6].noGravity = false;
			}
			else
			{
				Main.dust[num6].scale *= 0.6f;
			}
		}
		num2 = 1f;
	}

	private static void Spawn_CattivaHit(ParticleOrchestraSettings settings)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 6f;
		Vector2 val = new Vector2(0.7f);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			float num4 = (float)Math.PI * 2f * num3 + num + Main.rand.NextFloatDirection() * 0.25f;
			Vector2 val2 = 4.5f * val;
			float num5 = 16f;
			Vector2 val3 = Main.rand.NextVector2Circular(6f, 6f) * val;
			Color colorTint = Color.Lerp(Color.Gold, Color.Orange, Main.rand.NextFloat());
			if (Main.rand.Next(2) == 0)
			{
				colorTint = Color.Lerp(Color.White, Color.Orange, Main.rand.NextFloat());
			}
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = num4.ToRotationVector2() * val2;
			prettySparkleParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(val2 / num5) * 0.5f;
			prettySparkleParticle.ColorTint = colorTint;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val3;
			prettySparkleParticle.Rotation = num4;
			prettySparkleParticle.Scale = val * (Main.rand.NextFloat() * 0.7f + 0.1f);
			prettySparkleParticle.DrawVerticalAxis = false;
			prettySparkleParticle.FadeInEnd = 0.1f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.5f;
			prettySparkleParticle.TimeToLive = num5;
			prettySparkleParticle.Scale.X *= 4f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		num2 = 1f;
	}

	private static void Spawn_Chlorophyte(ParticleOrchestraSettings settings)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(228);
		RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
		randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[228], null, Vector2.Zero, Vector2.Zero);
		randomizedFrameParticle.SetTypeInfo(Main.projFrames[228], 6, 100f);
		randomizedFrameParticle.Velocity = settings.MovementVector;
		Vector2 val = -randomizedFrameParticle.Velocity / 100f;
		randomizedFrameParticle.AccelerationPerFrame = val + new Vector2(0f, -0.01f);
		randomizedFrameParticle.ColorTint = Color.White * 0.75f;
		randomizedFrameParticle.LocalPosition = settings.PositionInWorld;
		randomizedFrameParticle.Rotation = 0f;
		randomizedFrameParticle.FadeInNormalizedTime = 0.3f;
		randomizedFrameParticle.FadeOutNormalizedTime = 0.4f;
		randomizedFrameParticle.Scale = new Vector2(1.1f, 1.1f);
		Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
	}

	private static void Spawn_Keybrand(ParticleOrchestraSettings settings)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 3f;
		Vector2 val = new Vector2(0.7f);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			float num4 = (float)Math.PI * 2f * num3 + num + Main.rand.NextFloatDirection() * 0.1f;
			Vector2 val2 = 1.5f * val;
			float num5 = 60f;
			Vector2 val3 = Main.rand.NextVector2Circular(4f, 4f) * val;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = num4.ToRotationVector2() * val2;
			prettySparkleParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(val2 / num5);
			prettySparkleParticle.ColorTint = Color.Lerp(Color.Gold, Color.OrangeRed, Main.rand.NextFloat());
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val3;
			prettySparkleParticle.Rotation = num4;
			prettySparkleParticle.Scale = val * 0.8f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		num += 1f / num2 / 2f * ((float)Math.PI * 2f);
		num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		for (float num6 = 0f; num6 < 1f; num6 += 1f / num2)
		{
			float num7 = (float)Math.PI * 2f * num6 + num + Main.rand.NextFloatDirection() * 0.1f;
			Vector2 val4 = 1f * val;
			float num8 = 30f;
			Color val5 = Color.Lerp(Color.Gold, Color.OrangeRed, Main.rand.NextFloat());
			val5 = Color.Lerp(Color.White, val5, 0.5f);
			val5.A = 0;
			Vector2 val6 = Main.rand.NextVector2Circular(4f, 4f) * val;
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Extra[98], null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num8);
			fadingParticle.Velocity = num7.ToRotationVector2() * val4;
			fadingParticle.AccelerationPerFrame = num7.ToRotationVector2() * -(val4 / num8);
			fadingParticle.ColorTint = val5;
			fadingParticle.LocalPosition = settings.PositionInWorld + num7.ToRotationVector2() * val4 * val * num8 * 0.2f + val6;
			fadingParticle.Rotation = num7 + (float)Math.PI / 2f;
			fadingParticle.FadeInNormalizedTime = 0.3f;
			fadingParticle.FadeOutNormalizedTime = 0.4f;
			fadingParticle.Scale = new Vector2(0.5f, 1.2f) * 0.8f * val;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}
		num2 = 1f;
		num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		for (float num9 = 0f; num9 < 1f; num9 += 1f / num2)
		{
			float num10 = (float)Math.PI * 2f * num9 + num;
			float timeToLive = 30f;
			Color colorTint = Color.Lerp(Color.CornflowerBlue, Color.White, Main.rand.NextFloat());
			colorTint.A = 127;
			Vector2 val7 = Main.rand.NextVector2Circular(4f, 4f) * val;
			Vector2 val8 = Main.rand.NextVector2Square(0.7f, 1.3f);
			FadingParticle fadingParticle2 = _poolFading.RequestParticle();
			fadingParticle2.SetBasicInfo(TextureAssets.Extra[174], null, Vector2.Zero, Vector2.Zero);
			fadingParticle2.SetTypeInfo(timeToLive);
			fadingParticle2.ColorTint = colorTint;
			fadingParticle2.LocalPosition = settings.PositionInWorld + val7;
			fadingParticle2.Rotation = num10 + (float)Math.PI / 2f;
			fadingParticle2.FadeInNormalizedTime = 0.1f;
			fadingParticle2.FadeOutNormalizedTime = 0.4f;
			fadingParticle2.Scale = new Vector2(0.1f, 0.1f) * val;
			fadingParticle2.ScaleVelocity = val8 * 1f / 60f;
			fadingParticle2.ScaleAcceleration = val8 * (-1f / 60f) / 60f;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
		}
	}

	private static void Spawn_FlameWaders(ParticleOrchestraSettings settings)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		float num = 60f;
		for (int i = -1; i <= 1; i++)
		{
			int num2 = Main.rand.NextFromList(new short[3] { 326, 327, 328 });
			Main.instance.LoadProjectile(num2);
			Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
			float num3 = Main.rand.NextFloat() * 0.9f + 0.1f;
			Vector2 val = settings.PositionInWorld + new Vector2((float)i * 5.3333335f, 0f);
			FlameParticle flameParticle = _poolFlame.RequestParticle();
			flameParticle.SetBasicInfo(TextureAssets.Projectile[num2], null, Vector2.Zero, val);
			flameParticle.SetTypeInfo(num, settings.IndexOfPlayerWhoInvokedThis, player.cFlameWaker);
			flameParticle.FadeOutNormalizedTime = 0.4f;
			flameParticle.ScaleAcceleration = Vector2.One * num3 * (-1f / 60f) / num;
			flameParticle.Scale = Vector2.One * num3;
			Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);
			if (Main.rand.Next(16) == 0)
			{
				Dust dust = Dust.NewDustDirect(val, 4, 4, 6, 0f, 0f, 100);
				if (Main.rand.Next(2) == 0)
				{
					dust.noGravity = true;
					dust.fadeIn = 1.15f;
				}
				else
				{
					dust.scale = 0.6f;
				}
				dust.velocity *= 0.6f;
				dust.velocity.Y -= 1.2f;
				dust.noLight = true;
				dust.position.Y -= 4f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cFlameWaker, player);
			}
		}
	}

	private static void Spawn_WallOfFleshGoatMountFlames(ParticleOrchestraSettings settings)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		float num = 50f;
		for (int i = -1; i <= 1; i++)
		{
			int num2 = Main.rand.NextFromList(new short[3] { 326, 327, 328 });
			Main.instance.LoadProjectile(num2);
			Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
			float num3 = Main.rand.NextFloat() * 0.9f + 0.1f;
			Vector2 val = settings.PositionInWorld + new Vector2((float)i * 5.3333335f, 0f);
			FlameParticle flameParticle = _poolFlame.RequestParticle();
			flameParticle.SetBasicInfo(TextureAssets.Projectile[num2], null, Vector2.Zero, val);
			flameParticle.SetTypeInfo(num, settings.IndexOfPlayerWhoInvokedThis, player.cMount);
			flameParticle.FadeOutNormalizedTime = 0.3f;
			flameParticle.ScaleAcceleration = Vector2.One * num3 * (-1f / 60f) / num;
			flameParticle.Scale = Vector2.One * num3;
			Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);
			if (Main.rand.Next(8) == 0)
			{
				Dust dust = Dust.NewDustDirect(val, 4, 4, 6, 0f, 0f, 100);
				if (Main.rand.Next(2) == 0)
				{
					dust.noGravity = true;
					dust.fadeIn = 1.15f;
				}
				else
				{
					dust.scale = 0.6f;
				}
				dust.velocity *= 0.6f;
				dust.velocity.Y -= 1.2f;
				dust.noLight = true;
				dust.position.Y -= 4f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
			}
		}
	}
}
