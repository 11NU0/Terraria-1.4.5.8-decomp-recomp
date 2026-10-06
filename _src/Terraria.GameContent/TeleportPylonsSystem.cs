using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.Tile_Entities;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.GameContent;

public class TeleportPylonsSystem : IOnPlayerJoining
{
	private List<TeleportPylonInfo> _pylons = new List<TeleportPylonInfo>();

	private List<TeleportPylonInfo> _pylonsOld = new List<TeleportPylonInfo>();

	private int _cooldownForUpdatingPylonsList;

	private const int CooldownTimePerPylonsListUpdate = int.MaxValue;

	private SceneMetrics _sceneMetrics = new SceneMetrics();

	public List<TeleportPylonInfo> Pylons => _pylons;

	public void Update()
	{
		if (Main.netMode != 1)
		{
			if (_cooldownForUpdatingPylonsList > 0)
			{
				_cooldownForUpdatingPylonsList--;
				return;
			}
			_cooldownForUpdatingPylonsList = int.MaxValue;
			UpdatePylonsListAndBroadcastChanges();
		}
	}

	public bool HasPylonOfType(TeleportPylonType pylonType)
	{
		return _pylons.Any((TeleportPylonInfo x) => x.TypeOfPylon == pylonType);
	}

	public bool HasAnyPylon()
	{
		return _pylons.Count > 0;
	}

	public void RequestImmediateUpdate()
	{
		if (Main.netMode != 1)
		{
			_cooldownForUpdatingPylonsList = int.MaxValue;
			UpdatePylonsListAndBroadcastChanges();
		}
	}

	private void UpdatePylonsListAndBroadcastChanges()
	{
		Utils.Swap(ref _pylons, ref _pylonsOld);
		_pylons.Clear();
		foreach (TileEntity value in TileEntity.ByPosition.Values)
		{
			if (value is TETeleportationPylon tETeleportationPylon && tETeleportationPylon.TryGetPylonType(out var pylonType))
			{
				TeleportPylonInfo item = new TeleportPylonInfo
				{
					PositionInTiles = tETeleportationPylon.Position,
					TypeOfPylon = pylonType
				};
				_pylons.Add(item);
			}
		}
		IEnumerable<TeleportPylonInfo> enumerable = _pylonsOld.Except(_pylons);
		foreach (TeleportPylonInfo item2 in _pylons.Except(_pylonsOld))
		{
			NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(item2, NetTeleportPylonModule.SubPacketType.PylonWasAdded));
		}
		foreach (TeleportPylonInfo item3 in enumerable)
		{
			NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(item3, NetTeleportPylonModule.SubPacketType.PylonWasRemoved));
		}
	}

	public void AddForClient(TeleportPylonInfo info)
	{
		if (!_pylons.Contains(info))
		{
			_pylons.Add(info);
		}
	}

	public void RemoveForClient(TeleportPylonInfo info)
	{
		_pylons.RemoveAll((TeleportPylonInfo x) => x.Equals(info));
	}

	public void HandleTeleportRequest(TeleportPylonInfo info, int playerIndex)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[playerIndex];
		string key = null;
		bool flag = true;
		if (flag)
		{
			flag &= IsPlayerNearAPylon(player);
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon";
			}
		}
		if (flag)
		{
			int necessaryNPCCount = HowManyNPCsDoesPylonNeed(info, player);
			flag &= DoesPylonHaveEnoughNPCsAroundIt(info, necessaryNPCCount);
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecauseNotEnoughNPCs";
			}
		}
		if (flag)
		{
			if (!NPC.downedPlantBoss && (double)info.PositionInTiles.Y > Main.worldSurface && Framing.GetTileSafely(info.PositionInTiles.X, info.PositionInTiles.Y).wall == 87)
			{
				flag = false;
			}
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecauseAccessingLihzahrdTempleEarly";
			}
		}
		if (flag)
		{
			_sceneMetrics.Scan(new SceneMetricsScanSettings
			{
				BiomeScanCenterPositionInWorld = info.PositionInTiles.ToWorldCoordinates()
			});
			flag = DoesPylonAcceptTeleportation(info, player);
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements";
			}
		}
		if (flag)
		{
			bool flag2 = false;
			int num = 0;
			for (int i = 0; i < _pylons.Count; i++)
			{
				TeleportPylonInfo info2 = _pylons[i];
				if (!player.InTileEntityInteractionRange(info2.PositionInTiles.X, info2.PositionInTiles.Y, 3, 4, TileReachCheckSettings.Pylons))
				{
					continue;
				}
				if (num < 1)
				{
					num = 1;
				}
				int necessaryNPCCount2 = HowManyNPCsDoesPylonNeed(info2, player);
				if (DoesPylonHaveEnoughNPCsAroundIt(info2, necessaryNPCCount2))
				{
					if (num < 2)
					{
						num = 2;
					}
					_sceneMetrics.Scan(new SceneMetricsScanSettings
					{
						BiomeScanCenterPositionInWorld = info2.PositionInTiles.ToWorldCoordinates()
					});
					if (DoesPylonAcceptTeleportation(info2, player))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				flag = false;
				key = num switch
				{
					1 => "Net.CannotTeleportToPylonBecauseNotEnoughNPCsAtCurrentPylon", 
					2 => "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements", 
					_ => "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon", 
				};
			}
		}
		if (flag)
		{
			Vector2 val = info.PositionInTiles.ToWorldCoordinates() - new Vector2(0f, (float)player.HeightOffsetBoost);
			int num2 = 9;
			int typeOfPylon = (int)info.TypeOfPylon;
			int number = 0;
			player.Teleport(val, num2, typeOfPylon);
			player.velocity = Vector2.Zero;
			if (Main.netMode == 2)
			{
				RemoteClient.CheckSection(player.whoAmI, player.position);
				NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, val.X, val.Y, num2, number, typeOfPylon);
			}
		}
		else
		{
			ChatHelper.SendChatMessageToClient(NetworkText.FromKey(key), ChatColors.ServerMessage, playerIndex);
		}
	}

	public static bool IsPlayerNearAPylon(Player player)
	{
		return player.IsTileTypeInInteractionRange(597, TileReachCheckSettings.Pylons);
	}

	private bool DoesPylonHaveEnoughNPCsAroundIt(TeleportPylonInfo info, int necessaryNPCCount)
	{
		if (necessaryNPCCount <= 0)
		{
			return true;
		}
		Point16 positionInTiles = info.PositionInTiles;
		return DoesPositionHaveEnoughNPCs(necessaryNPCCount, positionInTiles);
	}

	public static bool DoesPositionHaveEnoughNPCs(int necessaryNPCCount, Point16 centerPoint)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = Utils.CenteredRectangle(centerPoint, SceneMetrics.ZoneScanSize);
		int num = necessaryNPCCount;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (!nPC.active || !nPC.isLikeATownNPC || nPC.homeless || !val.Contains(nPC.homeTileX, nPC.homeTileY))
			{
				continue;
			}
			Vector2 val2 = new Vector2((float)nPC.homeTileX, (float)nPC.homeTileY);
			Vector2 val3 = new Vector2(nPC.Center.X / 16f, nPC.Center.Y / 16f);
			if (Vector2.Distance(val2, val3) < 100f)
			{
				num--;
				if (num == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RequestTeleportation(TeleportPylonInfo info, Player player)
	{
		NetManager.Instance.SendToServerOrLoopback(NetTeleportPylonModule.SerializeUseRequest(info));
	}

	private bool DoesPylonAcceptTeleportation(TeleportPylonInfo info, Player player)
	{
		switch (info.TypeOfPylon)
		{
		case TeleportPylonType.SurfacePurity:
		{
			bool flag = (double)info.PositionInTiles.Y <= Main.worldSurface;
			if (Main.remixWorld)
			{
				flag = (double)info.PositionInTiles.Y > Main.rockLayer && info.PositionInTiles.Y < Main.maxTilesY - 350;
			}
			bool flag2 = info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380;
			if (!flag | flag2)
			{
				return false;
			}
			if (_sceneMetrics.EnoughTilesForJungle || _sceneMetrics.EnoughTilesForSnow || _sceneMetrics.EnoughTilesForDesert || _sceneMetrics.EnoughTilesForGlowingMushroom || _sceneMetrics.EnoughTilesForHallow || _sceneMetrics.EnoughTilesForCrimson || _sceneMetrics.EnoughTilesForCorruption)
			{
				return false;
			}
			return true;
		}
		case TeleportPylonType.Jungle:
			return _sceneMetrics.EnoughTilesForJungle;
		case TeleportPylonType.Snow:
			return _sceneMetrics.EnoughTilesForSnow;
		case TeleportPylonType.Desert:
			return _sceneMetrics.EnoughTilesForDesert;
		case TeleportPylonType.Beach:
		{
			bool flag3 = (double)info.PositionInTiles.Y <= Main.worldSurface && (double)info.PositionInTiles.Y > Main.worldSurface * 0.3499999940395355;
			bool flag4 = info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380;
			if (Main.remixWorld)
			{
				flag3 |= (double)info.PositionInTiles.Y > Main.rockLayer && info.PositionInTiles.Y < Main.maxTilesY - 350;
				flag4 |= (double)info.PositionInTiles.X < (double)Main.maxTilesX * 0.43 || (double)info.PositionInTiles.X > (double)Main.maxTilesX * 0.57;
			}
			return flag4 & flag3;
		}
		case TeleportPylonType.GlowingMushroom:
			if (Main.remixWorld && info.PositionInTiles.Y >= Main.maxTilesY - 200)
			{
				return false;
			}
			return _sceneMetrics.EnoughTilesForGlowingMushroom;
		case TeleportPylonType.Hallow:
			return _sceneMetrics.EnoughTilesForHallow;
		case TeleportPylonType.Underground:
			return (double)info.PositionInTiles.Y >= Main.worldSurface;
		case TeleportPylonType.Victory:
			return true;
		case TeleportPylonType.Underworld:
			return info.PositionInTiles.Y >= Main.UnderworldLayer;
		case TeleportPylonType.Shimmer:
			return _sceneMetrics.EnoughTilesForShimmer;
		default:
			return true;
		}
	}

	private int HowManyNPCsDoesPylonNeed(TeleportPylonInfo info, Player player)
	{
		TeleportPylonType typeOfPylon = info.TypeOfPylon;
		if (typeOfPylon != TeleportPylonType.Victory)
		{
			return 2;
		}
		return 0;
	}

	public void Reset()
	{
		_pylons.Clear();
		_cooldownForUpdatingPylonsList = 0;
	}

	public void OnPlayerJoining(int playerIndex)
	{
		foreach (TeleportPylonInfo pylon in _pylons)
		{
			NetManager.Instance.SendToClient(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(pylon, NetTeleportPylonModule.SubPacketType.PylonWasAdded), playerIndex);
		}
	}

	public static void SpawnInWorldDust(int tileStyle, Rectangle dustBox)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		float num2 = 1f;
		float num3 = 1f;
		switch ((TeleportPylonType)(byte)tileStyle)
		{
		case TeleportPylonType.SurfacePurity:
			num = 0.05f;
			num2 = 0.8f;
			num3 = 0.3f;
			break;
		case TeleportPylonType.Jungle:
			num = 0.7f;
			num2 = 0.8f;
			num3 = 0.05f;
			break;
		case TeleportPylonType.Hallow:
			num = 0.5f;
			num2 = 0.3f;
			num3 = 0.7f;
			break;
		case TeleportPylonType.Underground:
			num = 0.4f;
			num2 = 0.4f;
			num3 = 0.6f;
			break;
		case TeleportPylonType.Beach:
			num = 0.2f;
			num2 = 0.2f;
			num3 = 0.95f;
			break;
		case TeleportPylonType.Desert:
			num = 0.85f;
			num2 = 0.45f;
			num3 = 0.1f;
			break;
		case TeleportPylonType.Snow:
			num = 1f;
			num2 = 1f;
			num3 = 1.2f;
			break;
		case TeleportPylonType.GlowingMushroom:
			num = 0.4f;
			num2 = 0.7f;
			num3 = 1.2f;
			break;
		case TeleportPylonType.Victory:
			num = 0.7f;
			num2 = 0.7f;
			num3 = 0.7f;
			break;
		case TeleportPylonType.Underworld:
			num = 0.05f;
			num2 = 0.8f;
			num3 = 0.3f;
			break;
		case TeleportPylonType.Shimmer:
			num = 0.05f;
			num2 = 0.8f;
			num3 = 0.3f;
			break;
		}
		int num4 = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, 43, 0f, 0f, 254, new Color(num, num2, num3, 1f), 0.5f);
		Dust obj = Main.dust[num4];
		obj.velocity *= 0.1f;
		Main.dust[num4].velocity.Y -= 0.2f;
	}
}
