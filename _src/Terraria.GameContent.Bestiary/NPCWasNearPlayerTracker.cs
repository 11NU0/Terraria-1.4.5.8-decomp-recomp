using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Bestiary;

public class NPCWasNearPlayerTracker : IPersistentPerWorldContent, IOnPlayerJoining
{
	private object _entryCreationLock = new object();

	private HashSet<string> _wasNearPlayer;

	private List<Rectangle> _playerHitboxesForBestiary;

	private List<int> _wasSeenNearPlayerByNetId;

	public void PrepareSamplesBasedOptimizations()
	{
	}

	public NPCWasNearPlayerTracker()
	{
		_wasNearPlayer = new HashSet<string>();
		_playerHitboxesForBestiary = new List<Rectangle>();
		_wasSeenNearPlayerByNetId = new List<int>();
	}

	public void RegisterWasNearby(NPC npc)
	{
		string bestiaryCreditId = npc.GetBestiaryCreditId();
		bool flag = !_wasNearPlayer.Contains(bestiaryCreditId);
		SetWasSeenDirectly(bestiaryCreditId);
		if ((Main.netMode == 2) & flag)
		{
			NetManager.Instance.Broadcast(NetBestiaryModule.SerializeSight(npc.netID));
		}
	}

	public void SetWasSeenDirectly(string persistentId)
	{
		lock (_entryCreationLock)
		{
			if (_wasNearPlayer.Add(persistentId))
			{
				AchievementsHelper.TryGrantingBestiary100PercentAchievement();
			}
		}
	}

	public bool GetWasNearbyBefore(NPC npc)
	{
		string bestiaryCreditId = npc.GetBestiaryCreditId();
		return GetWasNearbyBefore(bestiaryCreditId);
	}

	public bool GetWasNearbyBefore(string persistentIdentifier)
	{
		return _wasNearPlayer.Contains(persistentIdentifier);
	}

	public void Save(BinaryWriter writer)
	{
		lock (_entryCreationLock)
		{
			writer.Write(_wasNearPlayer.Count);
			foreach (string item in _wasNearPlayer)
			{
				writer.Write(item);
			}
		}
	}

	public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string item = reader.ReadString();
			_wasNearPlayer.Add(item);
		}
	}

	public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			reader.ReadString();
		}
	}

	public void Reset()
	{
		_wasNearPlayer.Clear();
		_playerHitboxesForBestiary.Clear();
		_wasSeenNearPlayerByNetId.Clear();
	}

	public void ScanWorldForFinds()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		_playerHitboxesForBestiary.Clear();
		for (int i = 0; i < 255; i++)
		{
			Player player = Main.player[i];
			if (player.active)
			{
				_playerHitboxesForBestiary.Add(player.HitboxForBestiaryNearbyCheck);
			}
		}
		for (int j = 0; j < Main.maxNPCs; j++)
		{
			NPC nPC = Main.npc[j];
			if (!nPC.active || !nPC.CountsAsACritter || _wasSeenNearPlayerByNetId.Contains(nPC.netID))
			{
				continue;
			}
			Rectangle hitbox = nPC.Hitbox;
			for (int k = 0; k < _playerHitboxesForBestiary.Count; k++)
			{
				Rectangle val = _playerHitboxesForBestiary[k];
				if (hitbox.Intersects(val))
				{
					_wasSeenNearPlayerByNetId.Add(nPC.netID);
					RegisterWasNearby(nPC);
				}
			}
		}
	}

	public void OnPlayerJoining(int playerIndex)
	{
		foreach (string item in _wasNearPlayer)
		{
			if (ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(item, out var value))
			{
				NetManager.Instance.SendToClient(NetBestiaryModule.SerializeSight(value), playerIndex);
			}
		}
	}
}
