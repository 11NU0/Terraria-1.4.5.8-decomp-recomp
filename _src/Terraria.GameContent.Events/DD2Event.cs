using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Events;

public class DD2Event
{
	public class DamageTracker : NPCDamageTracker
	{
		private bool _won;

		public override LocalizedText Name => Language.GetText("Bestiary_Invasions.OldOnesArmy");

		public override LocalizedText KillTimeMessage => Language.GetText(_won ? "BossDamageCommand.KillTimeDefeated" : "BossDamageCommand.KillTimeLost");

		protected override bool IncludeDamageFor(NPC npc)
		{
			if (NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type])
			{
				return npc.type != 548;
			}
			return false;
		}

		public void Stop(bool won)
		{
			_won = won;
			Stop();
		}
	}

	private static readonly Color INFO_NEW_WAVE_COLOR = new Color(175, 55, 255);

	private static readonly Color INFO_START_INVASION_COLOR = ChatColors.World;

	private static readonly Color INFO_FAILURE_INVASION_COLOR = new Color(255, 0, 0);

	private const int INVASION_ID = 3;

	public static bool DownedInvasionT1;

	public static bool DownedInvasionT2;

	public static bool DownedInvasionT3;

	public static bool LostThisRun;

	public static bool WonThisRun;

	public static int LaneSpawnRate = 60;

	private static bool _downedDarkMageT1;

	private static bool _downedOgreT2;

	private static bool _spawnedBetsyT3;

	public static bool Ongoing;

	private static DamageTracker _damageTracker;

	public static Rectangle ArenaHitbox;

	private static int _arenaHitboxingCooldown;

	public static int OngoingDifficulty;

	private static List<Vector2> _deadGoblinSpots = new List<Vector2>();

	private static int _crystalsDropping_lastWave;

	private static int _crystalsDropping_toDrop;

	private static int _crystalsDropping_alreadyDropped;

	private static int _timeLeftUntilSpawningBegins;

	public static bool ReadyToFindBartender => NPC.downedBoss2;

	public static bool DownedInvasionAnyDifficulty
	{
		get
		{
			if (!DownedInvasionT1 && !DownedInvasionT2)
			{
				return DownedInvasionT3;
			}
			return true;
		}
	}

	public static int TimeLeftBetweenWaves
	{
		get
		{
			return _timeLeftUntilSpawningBegins;
		}
		set
		{
			_timeLeftUntilSpawningBegins = value;
		}
	}

	public static bool EnemySpawningIsOnHold => _timeLeftUntilSpawningBegins != 0;

	public static bool EnemiesShouldChasePlayers
	{
		get
		{
			if (!Ongoing)
			{
				return true;
			}
			return true;
		}
	}

	public static bool ReadyForTier2
	{
		get
		{
			if (Main.hardMode)
			{
				return NPC.downedMechBossAny;
			}
			return false;
		}
	}

	public static bool ReadyForTier3
	{
		get
		{
			if (Main.hardMode)
			{
				return NPC.downedGolemBoss;
			}
			return false;
		}
	}

	public static void Save(BinaryWriter writer)
	{
		writer.Write(DownedInvasionT1);
		writer.Write(DownedInvasionT2);
		writer.Write(DownedInvasionT3);
	}

	public static void Load(BinaryReader reader, int gameVersionNumber)
	{
		if (gameVersionNumber < 178)
		{
			NPC.savedBartender = false;
			ResetProgressEntirely();
			return;
		}
		NPC.savedBartender = reader.ReadBoolean();
		DownedInvasionT1 = reader.ReadBoolean();
		DownedInvasionT2 = reader.ReadBoolean();
		DownedInvasionT3 = reader.ReadBoolean();
	}

	public static void ResetProgressEntirely()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		DownedInvasionT1 = (DownedInvasionT2 = (DownedInvasionT3 = false));
		Ongoing = false;
		ArenaHitbox = default;
		_arenaHitboxingCooldown = 0;
		_timeLeftUntilSpawningBegins = 0;
		_damageTracker = null;
	}

	public static void ReportEventProgress()
	{
		GetInvasionStatus(out var currentWave, out var requiredKillCount, out var currentKillCount);
		Main.ReportInvasionProgress(currentKillCount, requiredKillCount, 3, currentWave);
	}

	public static void SyncInvasionProgress(int toWho)
	{
		GetInvasionStatus(out var currentWave, out var requiredKillCount, out var currentKillCount);
		NetMessage.SendData(78, toWho, -1, null, currentKillCount, requiredKillCount, 3f, currentWave);
	}

	public static void UpdateTime()
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		if (!Ongoing && !Main.dedServ)
		{
			Filters.Scene.Deactivate("CrystalDestructionVortex");
			Filters.Scene.Deactivate("CrystalDestructionColor");
			Filters.Scene.Deactivate("CrystalWin");
			return;
		}
		if (Main.netMode != 1 && !NPC.AnyNPCs(548))
		{
			StopInvasion();
		}
		if (Main.netMode == 1)
		{
			if (_timeLeftUntilSpawningBegins > 0)
			{
				_timeLeftUntilSpawningBegins--;
			}
			if (_timeLeftUntilSpawningBegins < 0)
			{
				_timeLeftUntilSpawningBegins = 0;
			}
			return;
		}
		if (_timeLeftUntilSpawningBegins > 0)
		{
			_timeLeftUntilSpawningBegins--;
			if (_timeLeftUntilSpawningBegins == 0)
			{
				GetInvasionStatus(out var currentWave, out var requiredKillCount, out var currentKillCount);
				if (!LostThisRun)
				{
					WorldGen.BroadcastText(Lang.GetInvasionWaveText(currentWave, GetEnemiesForWave(currentWave)), INFO_NEW_WAVE_COLOR);
					if (currentWave == 7 && OngoingDifficulty == 3)
					{
						SummonBetsy();
					}
				}
				else
				{
					LoseInvasionMessage();
				}
				if (Main.netMode != 1)
				{
					Main.ReportInvasionProgress(currentKillCount, requiredKillCount, 3, currentWave);
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, 3f, currentWave);
				}
			}
		}
		if (_timeLeftUntilSpawningBegins < 0)
		{
			_timeLeftUntilSpawningBegins = 0;
		}
	}

	public static void StartInvasion(int difficultyOverride = -1)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 1)
		{
			_crystalsDropping_toDrop = 0;
			_crystalsDropping_alreadyDropped = 0;
			_crystalsDropping_lastWave = 0;
			_timeLeftUntilSpawningBegins = 0;
			Ongoing = true;
			FindProperDifficulty();
			if (difficultyOverride != -1)
			{
				OngoingDifficulty = difficultyOverride;
			}
			_deadGoblinSpots.Clear();
			_downedDarkMageT1 = false;
			_downedOgreT2 = false;
			_spawnedBetsyT3 = false;
			LostThisRun = false;
			WonThisRun = false;
			NPC.totalInvasionPoints = 0f;
			NPC.waveKills = 0f;
			NPC.waveNumber = 1;
			ClearAllTowersInGame();
			NPCDamageTracker.Start(_damageTracker = new DamageTracker());
			WorldGen.BroadcastText(NetworkText.FromKey("DungeonDefenders2.InvasionStart"), INFO_START_INVASION_COLOR);
			NetMessage.SendData(7);
			if (Main.netMode != 1)
			{
				Main.ReportInvasionProgress(0, 1, 3, 1);
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(78, -1, -1, null, 0, 1f, 3f, 1f);
			}
			SetEnemySpawningOnHold(300);
			WipeEntities();
		}
	}

	public static void StopInvasion(bool win = false)
	{
		if (Ongoing)
		{
			if (win)
			{
				WinInvasionInternal();
			}
			Ongoing = false;
			_deadGoblinSpots.Clear();
			if (Main.netMode != 1)
			{
				NPC.totalInvasionPoints = 0f;
				NPC.waveKills = 0f;
				NPC.waveNumber = 0;
				WipeEntities();
				NetMessage.SendData(7);
			}
		}
	}

	private static void WinInvasionInternal()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (OngoingDifficulty >= 1)
		{
			DownedInvasionT1 = true;
		}
		if (OngoingDifficulty >= 2)
		{
			DownedInvasionT2 = true;
		}
		if (OngoingDifficulty >= 3)
		{
			DownedInvasionT3 = true;
		}
		if (OngoingDifficulty == 1)
		{
			DropMedals(3);
		}
		if (OngoingDifficulty == 2)
		{
			DropMedals(15);
		}
		if (OngoingDifficulty == 3)
		{
			AchievementsHelper.NotifyProgressionEvent(23);
			DropMedals(60);
		}
		WorldGen.BroadcastText(NetworkText.FromKey("DungeonDefenders2.InvasionWin"), INFO_START_INVASION_COLOR);
	}

	public static void LoseInvasionMessage()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		WorldGen.BroadcastText(NetworkText.FromKey("DungeonDefenders2.InvasionLose"), INFO_FAILURE_INVASION_COLOR);
	}

	private static void FindProperDifficulty()
	{
		OngoingDifficulty = 1;
		if (ReadyForTier2)
		{
			OngoingDifficulty = 2;
		}
		if (ReadyForTier3)
		{
			OngoingDifficulty = 3;
		}
	}

	public static void CheckProgress(int slainMonsterID)
	{
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1 || !Ongoing || LostThisRun || WonThisRun || EnemySpawningIsOnHold)
		{
			return;
		}
		GetInvasionStatus(out var currentWave, out var requiredKillCount, out var currentKillCount);
		float num = GetMonsterPointsWorth(slainMonsterID);
		float waveKills = NPC.waveKills;
		NPC.waveKills += num;
		NPC.totalInvasionPoints += num;
		currentKillCount += (int)num;
		bool flag = false;
		int num2 = currentWave;
		if (NPC.waveKills >= (float)requiredKillCount && requiredKillCount != 0)
		{
			NPC.waveKills = 0f;
			NPC.waveNumber++;
			flag = true;
			GetInvasionStatus(out currentWave, out requiredKillCount, out currentKillCount, currentlyInCheckProgress: true);
			if (WonThisRun)
			{
				if ((float)currentKillCount != waveKills && num != 0f)
				{
					if (Main.netMode != 1)
					{
						Main.ReportInvasionProgress(currentKillCount, requiredKillCount, 3, currentWave);
					}
					if (Main.netMode == 2)
					{
						NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, 3f, currentWave);
					}
				}
				return;
			}
			int num3 = currentWave;
			string key = "DungeonDefenders2.WaveComplete";
			if (num3 == 2)
			{
				key = "DungeonDefenders2.WaveCompleteFirst";
			}
			WorldGen.BroadcastText(NetworkText.FromKey(key), INFO_NEW_WAVE_COLOR);
			SetEnemySpawningOnHold(1800);
			if (OngoingDifficulty == 1)
			{
				if (num3 == 5)
				{
					DropMedals(1);
				}
				if (num3 == 4)
				{
					DropMedals(1);
				}
			}
			if (OngoingDifficulty == 2)
			{
				if (num3 == 7)
				{
					DropMedals(6);
				}
				if (num3 == 6)
				{
					DropMedals(3);
				}
				if (num3 == 5)
				{
					DropMedals(1);
				}
			}
			if (OngoingDifficulty == 3)
			{
				if (num3 == 7)
				{
					DropMedals(25);
				}
				if (num3 == 6)
				{
					DropMedals(11);
				}
				if (num3 == 5)
				{
					DropMedals(3);
				}
				if (num3 == 4)
				{
					DropMedals(1);
				}
			}
		}
		if ((float)currentKillCount == waveKills)
		{
			return;
		}
		if (flag)
		{
			int num4 = 1;
			int num5 = 1;
			if (Main.netMode != 1)
			{
				Main.ReportInvasionProgress(num4, num5, 3, num2);
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(78, -1, -1, null, num4, num5, 3f, num2);
			}
		}
		else
		{
			if (Main.netMode != 1)
			{
				Main.ReportInvasionProgress(currentKillCount, requiredKillCount, 3, currentWave);
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, 3f, currentWave);
			}
		}
	}

	public static void StartVictoryScene()
	{
		WonThisRun = true;
		if (_damageTracker != null)
		{
			_damageTracker.Stop(won: true);
		}
		int num = NPC.FindFirstNPC(548);
		if (num == -1)
		{
			return;
		}
		Main.npc[num].ai[1] = 2f;
		Main.npc[num].ai[0] = 2f;
		Main.npc[num].netUpdate = true;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			if (Main.npc[i] != null && Main.npc[i].active && Main.npc[i].type == 549)
			{
				Main.npc[i].ai[0] = 0f;
				Main.npc[i].ai[1] = 1f;
				Main.npc[i].netUpdate = true;
			}
		}
	}

	public static void ReportLoss()
	{
		if (_damageTracker != null)
		{
			_damageTracker.Stop(won: false);
		}
		LostThisRun = true;
		SetEnemySpawningOnHold(30);
	}

	private static void GetInvasionStatus(out int currentWave, out int requiredKillCount, out int currentKillCount, bool currentlyInCheckProgress = false)
	{
		currentWave = NPC.waveNumber;
		requiredKillCount = 10;
		currentKillCount = (int)NPC.waveKills;
		switch (OngoingDifficulty)
		{
		case 3:
			requiredKillCount = Difficulty_3_GetRequiredWaveKills(ref currentWave, ref currentKillCount, currentlyInCheckProgress);
			break;
		case 2:
			requiredKillCount = Difficulty_2_GetRequiredWaveKills(ref currentWave, ref currentKillCount, currentlyInCheckProgress);
			break;
		default:
			requiredKillCount = Difficulty_1_GetRequiredWaveKills(ref currentWave, ref currentKillCount, currentlyInCheckProgress);
			break;
		}
	}

	private static short[] GetEnemiesForWave(int wave)
	{
		return OngoingDifficulty switch
		{
			3 => Difficulty_3_GetEnemiesForWave(wave), 
			2 => Difficulty_2_GetEnemiesForWave(wave), 
			_ => Difficulty_1_GetEnemiesForWave(wave), 
		};
	}

	private static int GetMonsterPointsWorth(int slainMonsterID)
	{
		return OngoingDifficulty switch
		{
			3 => Difficulty_3_GetMonsterPointsWorth(slainMonsterID), 
			2 => Difficulty_2_GetMonsterPointsWorth(slainMonsterID), 
			_ => Difficulty_1_GetMonsterPointsWorth(slainMonsterID), 
		};
	}

	public static void SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		switch (OngoingDifficulty)
		{
		case 3:
			Difficulty_3_SpawnMonsterFromGate(gateBottom, leftGate);
			break;
		case 2:
			Difficulty_2_SpawnMonsterFromGate(gateBottom, leftGate);
			break;
		default:
			Difficulty_1_SpawnMonsterFromGate(gateBottom, leftGate);
			break;
		}
	}

	public static void SummonCrystal(int x, int y, int whoAsks)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendData(113, -1, -1, null, x, y);
		}
		else
		{
			SummonCrystalDirect(x, y, whoAsks);
		}
	}

	public static void SummonCrystalDirect(int x, int y, int whoAsks)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		if (!NPC.AnyNPCs(548))
		{
			Tile tileSafely = Framing.GetTileSafely(x, y);
			if (tileSafely.active() && tileSafely.type == 466)
			{
				Point val = new Point(x * 16, y * 16);
				val.X -= tileSafely.frameX / 18 * 16;
				val.Y -= tileSafely.frameY / 18 * 16;
				val.X += 40;
				val.Y += 64;
				StartInvasion();
				NPC.NewNPC(Main.player[whoAsks].GetNPCSource_TileInteraction(x, y), val.X, val.Y, 548);
				DropStarterCrystals();
			}
		}
	}

	public static bool WouldFailSpawningHere(int x, int y)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		StrayMethods.CheckArenaScore(new Point(x, y).ToWorldCoordinates(), out var xLeftEnd, out var xRightEnd);
		int num = xRightEnd.X - x;
		int num2 = x - xLeftEnd.X;
		if (num < 60 || num2 < 60)
		{
			return true;
		}
		return false;
	}

	public static void FailureMessage(int client)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		LocalizedText text = Language.GetText("DungeonDefenders2.BartenderWarning");
		Color color = new Color(255, 255, 0);
		if (Main.netMode == 2)
		{
			ChatHelper.SendChatMessageToClient(NetworkText.FromKey(text.Key), color, client);
		}
		else
		{
			Main.NewText(text.Value, color.R, color.G, color.B);
		}
	}

	public static void WipeEntities()
	{
		ClearAllTowersInGame();
		ClearAllDD2HostilesInGame();
		ClearAllDD2EnergyCrystalsInChests();
		if (Main.netMode == 2)
		{
			NetMessage.SendData(114);
		}
	}

	public static void ClearAllTowersInGame()
	{
		for (int i = 0; i < 1000; i++)
		{
			if (Main.projectile[i].active && ProjectileID.Sets.IsADD2Turret[Main.projectile[i].type])
			{
				Main.projectile[i].Kill();
			}
		}
	}

	public static void ClearAllDD2HostilesInGame()
	{
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			if (Main.npc[i].active && NPCID.Sets.BelongsToInvasionOldOnesArmy[Main.npc[i].type])
			{
				Main.npc[i].active = false;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(23, -1, -1, null, i);
				}
			}
		}
	}

	public static void ClearAllDD2EnergyCrystalsInChests()
	{
		if (Main.netMode == 1)
		{
			return;
		}
		List<int> currentlyOpenChests = Chest.GetCurrentlyOpenChests();
		for (int i = 0; i < 8000; i++)
		{
			Chest chest = Main.chest[i];
			if (chest == null || !currentlyOpenChests.Contains(i))
			{
				continue;
			}
			for (int j = 0; j < chest.maxItems; j++)
			{
				if (chest.item[j].type == 3822 && chest.item[j].stack > 0)
				{
					chest.item[j].TurnToAir();
					if (Main.netMode != 0)
					{
						NetMessage.SendData(32, -1, -1, null, i, j);
					}
				}
			}
		}
	}

	public static void AnnounceGoblinDeath(NPC n)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_deadGoblinSpots.Add(n.Bottom);
	}

	public static bool CanRaiseGoblinsHere(Vector2 spot)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (Vector2 deadGoblinSpot in _deadGoblinSpots)
		{
			if (Vector2.DistanceSquared(deadGoblinSpot, spot) <= 640000f)
			{
				num++;
				if (num >= 3)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void RaiseGoblins(NPC caller, Vector2 spot)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		List<Vector2> list = new List<Vector2>();
		foreach (Vector2 deadGoblinSpot in _deadGoblinSpots)
		{
			if (Vector2.DistanceSquared(deadGoblinSpot, spot) <= 722500f)
			{
				list.Add(deadGoblinSpot);
			}
		}
		foreach (Vector2 item in list)
		{
			_deadGoblinSpots.Remove(item);
		}
		int num = 0;
		foreach (Vector2 item2 in list)
		{
			Point origin = item2.ToTileCoordinates();
			origin.X += Main.rand.Next(-15, 16);
			if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(50), new Conditions.IsSolid()), out var result))
			{
				if (OngoingDifficulty == 3)
				{
					NPC.NewNPC(caller.GetSpawnSourceForNPCFromNPCAI(), result.X * 16 + 8, result.Y * 16, 567);
				}
				else
				{
					NPC.NewNPC(caller.GetSpawnSourceForNPCFromNPCAI(), result.X * 16 + 8, result.Y * 16, 566);
				}
				if (++num >= 8)
				{
					break;
				}
			}
		}
	}

	public static void FindArenaHitbox()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		if (_arenaHitboxingCooldown > 0)
		{
			_arenaHitboxingCooldown--;
			return;
		}
		_arenaHitboxingCooldown = 60;
		Vector2 val = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 val2 = new Vector2(0f, 0f);
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active && (nPC.type == 549 || nPC.type == 548))
			{
				Vector2 topLeft = nPC.TopLeft;
				if (val.X > topLeft.X)
				{
					val.X = topLeft.X;
				}
				if (val.Y > topLeft.Y)
				{
					val.Y = topLeft.Y;
				}
				topLeft = nPC.BottomRight;
				if (val2.X < topLeft.X)
				{
					val2.X = topLeft.X;
				}
				if (val2.Y < topLeft.Y)
				{
					val2.Y = topLeft.Y;
				}
			}
		}
		Vector2 val3 = new Vector2(16f, 16f) * 50f;
		val -= val3;
		val2 += val3;
		Vector2 val4 = val2 - val;
		ArenaHitbox.X = (int)val.X;
		ArenaHitbox.Y = (int)val.Y;
		ArenaHitbox.Width = (int)val4.X;
		ArenaHitbox.Height = (int)val4.Y;
	}

	public static bool ShouldBlockBuilding(Vector2 worldPosition)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ArenaHitbox.Contains(worldPosition.ToPoint());
	}

	public static void DropMedals(int numberOfMedals)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active && nPC.type == 548)
			{
				int num = Item.NewItem(nPC.GetItemSource_Loot(), (int)nPC.position.X, (int)nPC.position.Y, (int)nPC.Size.X, (int)nPC.Size.Y, 3817, numberOfMedals, noBroadcast: true);
				Main.item[num].MakeInstanced((Player p) => true);
			}
		}
	}

	public static bool ShouldDropCrystals()
	{
		GetInvasionStatus(out var currentWave, out var requiredKillCount, out var currentKillCount);
		if (_crystalsDropping_lastWave < currentWave)
		{
			_crystalsDropping_lastWave++;
			if (_crystalsDropping_alreadyDropped > 0)
			{
				_crystalsDropping_alreadyDropped -= _crystalsDropping_toDrop;
			}
			if (OngoingDifficulty == 1)
			{
				switch (currentWave)
				{
				case 1:
					_crystalsDropping_toDrop = 20;
					break;
				case 2:
					_crystalsDropping_toDrop = 20;
					break;
				case 3:
					_crystalsDropping_toDrop = 30;
					break;
				case 4:
					_crystalsDropping_toDrop = 30;
					break;
				case 5:
					_crystalsDropping_toDrop = 40;
					break;
				}
			}
			else if (OngoingDifficulty == 2)
			{
				switch (currentWave)
				{
				case 1:
					_crystalsDropping_toDrop = 20;
					break;
				case 2:
					_crystalsDropping_toDrop = 20;
					break;
				case 3:
					_crystalsDropping_toDrop = 20;
					break;
				case 4:
					_crystalsDropping_toDrop = 20;
					break;
				case 5:
					_crystalsDropping_toDrop = 20;
					break;
				case 6:
					_crystalsDropping_toDrop = 30;
					break;
				case 7:
					_crystalsDropping_toDrop = 30;
					break;
				}
			}
			else if (OngoingDifficulty == 3)
			{
				switch (currentWave)
				{
				case 1:
					_crystalsDropping_toDrop = 20;
					break;
				case 2:
					_crystalsDropping_toDrop = 20;
					break;
				case 3:
					_crystalsDropping_toDrop = 20;
					break;
				case 4:
					_crystalsDropping_toDrop = 20;
					break;
				case 5:
					_crystalsDropping_toDrop = 30;
					break;
				case 6:
					_crystalsDropping_toDrop = 30;
					break;
				case 7:
					_crystalsDropping_toDrop = 30;
					break;
				}
			}
		}
		if (Main.netMode != 0 && Main.expertMode)
		{
			_crystalsDropping_toDrop = (int)((float)_crystalsDropping_toDrop * NPC.GetBalance());
		}
		float num = (float)currentKillCount / (float)requiredKillCount;
		if ((float)_crystalsDropping_alreadyDropped < (float)_crystalsDropping_toDrop * num)
		{
			_crystalsDropping_alreadyDropped++;
			return true;
		}
		return false;
	}

	private static void SummonBetsy()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (!_spawnedBetsyT3 && !NPC.AnyNPCs(551))
		{
			Vector2 position = new Vector2(1f, 1f);
			int num = NPC.FindFirstNPC(548);
			if (num != -1)
			{
				position = Main.npc[num].Center;
			}
			NPC.SpawnOnPlayer(Player.FindClosest(position, 1, 1), 551);
			_spawnedBetsyT3 = true;
		}
	}

	private static void DropStarterCrystals()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			if (Main.npc[i].active && Main.npc[i].type == 548)
			{
				for (int j = 0; j < 5; j++)
				{
					Item.NewItem(new EntitySource_WorldEvent(), Main.npc[i].Center, 3822, 2);
				}
				break;
			}
		}
	}

	private static void SetEnemySpawningOnHold(int forHowLong)
	{
		_timeLeftUntilSpawningBegins = forHowLong;
		if (Main.netMode == 2)
		{
			NetMessage.SendData(116, -1, -1, null, _timeLeftUntilSpawningBegins);
		}
	}

	private static short[] Difficulty_1_GetEnemiesForWave(int wave)
	{
		LaneSpawnRate = 60;
		switch (wave)
		{
		case 1:
			LaneSpawnRate = 90;
			return new short[1] { 552 };
		case 2:
			return new short[2] { 552, 555 };
		case 3:
			LaneSpawnRate = 55;
			return new short[3] { 552, 555, 561 };
		case 4:
			LaneSpawnRate = 50;
			return new short[4] { 552, 555, 561, 558 };
		case 5:
			LaneSpawnRate = 40;
			return new short[5] { 552, 555, 561, 558, 564 };
		default:
			return new short[1] { 552 };
		}
	}

	private static int Difficulty_1_GetRequiredWaveKills(ref int waveNumber, ref int currentKillCount, bool currentlyInCheckProgress)
	{
		switch (waveNumber)
		{
		case -1:
			return 0;
		case 1:
			return 60;
		case 2:
			return 80;
		case 3:
			return 100;
		case 4:
			_deadGoblinSpots.Clear();
			return 120;
		case 5:
			if (!_downedDarkMageT1 && currentKillCount > 139)
			{
				currentKillCount = 139;
			}
			return 140;
		case 6:
			waveNumber = 5;
			currentKillCount = 1;
			if (currentlyInCheckProgress)
			{
				StartVictoryScene();
			}
			return 1;
		default:
			return 10;
		}
	}

	private static void Difficulty_1_SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		int x = (int)gateBottom.X;
		int y = (int)gateBottom.Y;
		int num = 50;
		int num2 = 6;
		if (NPC.waveNumber > 4)
		{
			num2 = 12;
		}
		else if (NPC.waveNumber > 3)
		{
			num2 = 8;
		}
		int num3 = 6;
		if (NPC.waveNumber > 4)
		{
			num3 = 8;
		}
		for (int i = 1; i < Main.CurrentFrameFlags.ActivePlayersCount; i++)
		{
			num = (int)((double)num * 1.3);
			num2 = (int)((double)num2 * 1.3);
			num3 = (int)((double)num3 * 1.3);
		}
		int num4 = Main.maxNPCs;
		switch (NPC.waveNumber)
		{
		case 1:
			if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
			{
				num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 552);
			}
			break;
		case 2:
			if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
			{
				num4 = ((Main.rand.Next(7) != 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 552) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 555));
			}
			break;
		case 3:
			if (Main.rand.Next(6) == 0 && NPC.CountNPCS(561) < num2)
			{
				num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 561);
			}
			else if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
			{
				num4 = ((Main.rand.Next(5) != 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 552) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 555));
			}
			break;
		case 4:
			if (Main.rand.Next(12) == 0 && NPC.CountNPCS(558) < num3)
			{
				num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 558);
			}
			else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(561) < num2)
			{
				num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 561);
			}
			else if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
			{
				num4 = ((Main.rand.Next(5) != 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 552) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 555));
			}
			break;
		case 5:
		{
			GetInvasionStatus(out var _, out var requiredKillCount, out var currentKillCount);
			if ((!leftGate || Main.rand.Next(2) == 0) && (float)currentKillCount > (float)requiredKillCount * 0.5f && !NPC.AnyNPCs(564))
			{
				num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 564);
			}
			if (Main.rand.Next(10) == 0 && NPC.CountNPCS(558) < num3)
			{
				num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 558);
			}
			else if (Main.rand.Next(4) == 0 && NPC.CountNPCS(561) < num2)
			{
				num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 561);
			}
			else if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
			{
				num4 = ((Main.rand.Next(4) != 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 552) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 555));
			}
			break;
		}
		default:
			num4 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 552);
			break;
		}
		if (Main.netMode == 2 && num4 < Main.maxNPCs)
		{
			NetMessage.SendData(23, -1, -1, null, num4);
		}
	}

	private static int Difficulty_1_GetMonsterPointsWorth(int slainMonsterID)
	{
		if (NPC.waveNumber == 5 && NPC.waveKills >= 139f)
		{
			if (slainMonsterID == 564 || slainMonsterID == 565)
			{
				_downedDarkMageT1 = true;
				return 1;
			}
			return 0;
		}
		if ((uint)(slainMonsterID - 551) <= 14u || (uint)(slainMonsterID - 568) <= 10u)
		{
			if (NPC.waveNumber == 5 && NPC.waveKills == 138f)
			{
				return 1;
			}
			if (!Main.expertMode)
			{
				return 1;
			}
			return 2;
		}
		return 0;
	}

	private static short[] Difficulty_2_GetEnemiesForWave(int wave)
	{
		LaneSpawnRate = 60;
		switch (wave)
		{
		case 1:
			LaneSpawnRate = 90;
			return new short[2] { 553, 562 };
		case 2:
			LaneSpawnRate = 70;
			return new short[3] { 553, 562, 572 };
		case 3:
			return new short[5] { 553, 556, 562, 559, 572 };
		case 4:
			LaneSpawnRate = 55;
			return new short[5] { 553, 559, 570, 572, 562 };
		case 5:
			LaneSpawnRate = 50;
			return new short[6] { 553, 556, 559, 572, 574, 570 };
		case 6:
			LaneSpawnRate = 45;
			return new short[8] { 553, 556, 562, 559, 568, 570, 572, 574 };
		case 7:
			LaneSpawnRate = 42;
			return new short[8] { 553, 556, 572, 559, 568, 574, 570, 576 };
		default:
			return new short[1] { 553 };
		}
	}

	private static int Difficulty_2_GetRequiredWaveKills(ref int waveNumber, ref int currentKillCount, bool currentlyInCheckProgress)
	{
		switch (waveNumber)
		{
		case -1:
			return 0;
		case 1:
			return 60;
		case 2:
			return 80;
		case 3:
			return 100;
		case 4:
			return 120;
		case 5:
			return 140;
		case 6:
			return 180;
		case 7:
			if (!_downedOgreT2 && currentKillCount > 219)
			{
				currentKillCount = 219;
			}
			return 220;
		case 8:
			waveNumber = 7;
			currentKillCount = 1;
			if (currentlyInCheckProgress)
			{
				StartVictoryScene();
			}
			return 1;
		default:
			return 10;
		}
	}

	private static int Difficulty_2_GetMonsterPointsWorth(int slainMonsterID)
	{
		if (NPC.waveNumber == 7 && NPC.waveKills >= 219f)
		{
			if (slainMonsterID == 576 || slainMonsterID == 577)
			{
				_downedOgreT2 = true;
				return 1;
			}
			return 0;
		}
		if ((uint)(slainMonsterID - 551) <= 14u || (uint)(slainMonsterID - 568) <= 10u)
		{
			if (NPC.waveNumber == 7 && NPC.waveKills == 218f)
			{
				return 1;
			}
			if (!Main.expertMode)
			{
				return 1;
			}
			return 2;
		}
		return 0;
	}

	private static void Difficulty_2_SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		int x = (int)gateBottom.X;
		int y = (int)gateBottom.Y;
		int num = 50;
		int num2 = 5;
		if (NPC.waveNumber > 1)
		{
			num2 = 8;
		}
		if (NPC.waveNumber > 3)
		{
			num2 = 10;
		}
		if (NPC.waveNumber > 5)
		{
			num2 = 12;
		}
		int num3 = 5;
		if (NPC.waveNumber > 4)
		{
			num3 = 7;
		}
		int num4 = 2;
		int num5 = 8;
		if (NPC.waveNumber > 3)
		{
			num5 = 12;
		}
		int num6 = 3;
		if (NPC.waveNumber > 5)
		{
			num6 = 5;
		}
		for (int i = 1; i < Main.CurrentFrameFlags.ActivePlayersCount; i++)
		{
			num = (int)((double)num * 1.3);
			num2 = (int)((double)num2 * 1.3);
			num5 = (int)((double)num * 1.3);
			num6 = (int)((double)num * 1.35);
		}
		int num7 = Main.maxNPCs;
		int num8 = Main.maxNPCs;
		switch (NPC.waveNumber)
		{
		case 1:
			if (Main.rand.Next(20) == 0 && NPC.CountNPCS(562) < num2)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 562);
			}
			else if (NPC.CountNPCS(553) < num)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			}
			break;
		case 2:
			if (Main.rand.Next(3) == 0 && NPC.CountNPCS(572) < num5)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 572);
			}
			else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(562) < num2)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 562);
			}
			else if (NPC.CountNPCS(553) < num)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			}
			break;
		case 3:
			if (Main.rand.Next(7) == 0 && NPC.CountNPCS(572) < num5)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 572);
			}
			else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(559) < num3)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 559);
			}
			else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(562) < num2)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 562);
			}
			else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
			{
				if (Main.rand.Next(4) == 0)
				{
					num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 556);
				}
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			}
			break;
		case 4:
			if (Main.rand.Next(10) == 0 && NPC.CountNPCS(570) < num6)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 570);
			}
			else if (Main.rand.Next(12) == 0 && NPC.CountNPCS(559) < num3)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 559);
			}
			else if (Main.rand.Next(6) == 0 && NPC.CountNPCS(562) < num2)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 562);
			}
			else if (Main.rand.Next(3) == 0 && NPC.CountNPCS(572) < num5)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 572);
			}
			else if (NPC.CountNPCS(553) < num)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			}
			break;
		case 5:
			if (Main.rand.Next(7) == 0 && NPC.CountNPCS(570) < num6)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 570);
			}
			else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(559) < num3)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 559);
			}
			else if (Main.rand.Next(4) == 0 && NPC.CountNPCS(572) + NPC.CountNPCS(574) < num5)
			{
				num7 = ((Main.rand.Next(2) != 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 574) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 572));
			}
			else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
			{
				if (Main.rand.Next(3) == 0)
				{
					num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 556);
				}
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			}
			break;
		case 6:
			if (Main.rand.Next(7) == 0 && NPC.CountNPCS(570) < num6)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 570);
			}
			else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(568) < num4)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 568);
			}
			else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(572) + NPC.CountNPCS(574) < num5)
			{
				num7 = ((Main.rand.Next(2) == 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 574) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 572));
			}
			else if (Main.rand.Next(9) == 0 && NPC.CountNPCS(559) < num3)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 559);
			}
			else if (Main.rand.Next(3) == 0 && NPC.CountNPCS(562) < num2)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 562);
			}
			else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
			{
				if (Main.rand.Next(3) != 0)
				{
					num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 556);
				}
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			}
			break;
		case 7:
		{
			GetInvasionStatus(out var _, out var requiredKillCount, out var currentKillCount);
			if ((!leftGate || Main.rand.Next(2) == 0) && (float)currentKillCount > (float)requiredKillCount * 0.1f && !NPC.AnyNPCs(576))
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 576);
			}
			else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(570) < num6)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 570);
			}
			else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(568) < num4)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 568);
			}
			else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(572) + NPC.CountNPCS(574) < num5)
			{
				num7 = ((Main.rand.Next(3) == 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 574) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 572));
			}
			else if (Main.rand.Next(11) == 0 && NPC.CountNPCS(559) < num3)
			{
				num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 559);
			}
			else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
			{
				if (Main.rand.Next(2) == 0)
				{
					num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 556);
				}
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			}
			break;
		}
		default:
			num7 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 553);
			break;
		}
		if (Main.netMode == 2 && num7 < Main.maxNPCs)
		{
			NetMessage.SendData(23, -1, -1, null, num7);
		}
		if (Main.netMode == 2 && num8 < Main.maxNPCs)
		{
			NetMessage.SendData(23, -1, -1, null, num8);
		}
	}

	private static short[] Difficulty_3_GetEnemiesForWave(int wave)
	{
		LaneSpawnRate = 60;
		switch (wave)
		{
		case 1:
			LaneSpawnRate = 85;
			return new short[3] { 554, 557, 563 };
		case 2:
			LaneSpawnRate = 75;
			return new short[5] { 554, 557, 563, 573, 578 };
		case 3:
			LaneSpawnRate = 60;
			return new short[5] { 554, 563, 560, 573, 571 };
		case 4:
			LaneSpawnRate = 60;
			return new short[7] { 554, 560, 571, 573, 563, 575, 565 };
		case 5:
			LaneSpawnRate = 55;
			return new short[7] { 554, 557, 573, 575, 571, 569, 577 };
		case 6:
			LaneSpawnRate = 60;
			return new short[8] { 554, 557, 563, 578, 569, 571, 577, 565 };
		case 7:
			LaneSpawnRate = 90;
			return new short[6] { 554, 557, 563, 569, 571, 551 };
		default:
			return new short[1] { 554 };
		}
	}

	private static int Difficulty_3_GetRequiredWaveKills(ref int waveNumber, ref int currentKillCount, bool currentlyInCheckProgress)
	{
		switch (waveNumber)
		{
		case -1:
			return 0;
		case 1:
			return 60;
		case 2:
			return 80;
		case 3:
			return 100;
		case 4:
			return 120;
		case 5:
			return 140;
		case 6:
			return 180;
		case 7:
		{
			int num = NPC.FindFirstNPC(551);
			if (num == -1)
			{
				return 1;
			}
			currentKillCount = 100 - (int)((float)Main.npc[num].life / (float)Main.npc[num].lifeMax * 100f);
			return 100;
		}
		case 8:
			waveNumber = 7;
			currentKillCount = 1;
			if (currentlyInCheckProgress)
			{
				StartVictoryScene();
			}
			return 1;
		default:
			return 10;
		}
	}

	private static int Difficulty_3_GetMonsterPointsWorth(int slainMonsterID)
	{
		if (NPC.waveNumber == 7)
		{
			if (slainMonsterID == 551)
			{
				return 1;
			}
			return 0;
		}
		if ((uint)(slainMonsterID - 551) <= 14u || (uint)(slainMonsterID - 568) <= 10u)
		{
			if (!Main.expertMode)
			{
				return 1;
			}
			return 2;
		}
		return 0;
	}

	private static void Difficulty_3_SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		int x = (int)gateBottom.X;
		int y = (int)gateBottom.Y;
		int num = 60;
		int num2 = 7;
		if (NPC.waveNumber > 1)
		{
			num2 = 9;
		}
		if (NPC.waveNumber > 3)
		{
			num2 = 12;
		}
		if (NPC.waveNumber > 5)
		{
			num2 = 15;
		}
		int num3 = 7;
		if (NPC.waveNumber > 4)
		{
			num3 = 10;
		}
		int num4 = 2;
		if (NPC.waveNumber > 5)
		{
			num4 = 3;
		}
		int num5 = 12;
		if (NPC.waveNumber > 3)
		{
			num5 = 18;
		}
		int num6 = 4;
		if (NPC.waveNumber > 5)
		{
			num6 = 6;
		}
		int num7 = 4;
		for (int i = 1; i < Main.CurrentFrameFlags.ActivePlayersCount; i++)
		{
			num = (int)((double)num * 1.3);
			num2 = (int)((double)num2 * 1.3);
			num5 = (int)((double)num * 1.3);
			num6 = (int)((double)num * 1.35);
			num7 = (int)((double)num7 * 1.3);
		}
		int num8 = Main.maxNPCs;
		int num9 = Main.maxNPCs;
		switch (NPC.waveNumber)
		{
		case 1:
			if (Main.rand.Next(18) == 0 && NPC.CountNPCS(563) < num2)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 563);
			}
			else if (NPC.CountNPCS(554) < num)
			{
				if (Main.rand.Next(7) == 0)
				{
					num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 557);
				}
				num9 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			}
			break;
		case 2:
			if (Main.rand.Next(3) == 0 && NPC.CountNPCS(578) < num7)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 578);
			}
			else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(563) < num2)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 563);
			}
			else if (Main.rand.Next(3) == 0 && NPC.CountNPCS(573) < num5)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 573);
			}
			else if (NPC.CountNPCS(554) < num)
			{
				if (Main.rand.Next(4) == 0)
				{
					num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 557);
				}
				num9 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			}
			break;
		case 3:
			if (Main.rand.Next(13) == 0 && NPC.CountNPCS(571) < num6)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 571);
			}
			else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(573) < num5)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 573);
			}
			else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(560) < num3)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 560);
			}
			else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(563) < num2)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 563);
			}
			else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			}
			break;
		case 4:
			if (Main.rand.Next(24) == 0 && !NPC.AnyNPCs(565))
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 565);
			}
			else if (Main.rand.Next(12) == 0 && NPC.CountNPCS(571) < num6)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 571);
			}
			else if (Main.rand.Next(15) == 0 && NPC.CountNPCS(560) < num3)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 560);
			}
			else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(563) < num2)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 563);
			}
			else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(573) + NPC.CountNPCS(575) < num5)
			{
				num8 = ((Main.rand.Next(3) == 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 575) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 573));
			}
			else if (NPC.CountNPCS(554) < num)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			}
			break;
		case 5:
			if (Main.rand.Next(20) == 0 && !NPC.AnyNPCs(577))
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 577);
			}
			else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(569) < num4)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 569);
			}
			else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(571) < num6)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 571);
			}
			else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(573) + NPC.CountNPCS(575) < num5)
			{
				num8 = ((Main.rand.Next(4) == 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 575) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 573));
			}
			else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
			{
				if (Main.rand.Next(3) == 0)
				{
					num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 557);
				}
				num9 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			}
			break;
		case 6:
			if (Main.rand.Next(20) == 0 && !NPC.AnyNPCs(577))
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 577);
			}
			else if (Main.rand.Next(20) == 0 && !NPC.AnyNPCs(565))
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 565);
			}
			else if (Main.rand.Next(12) == 0 && NPC.CountNPCS(571) < num6)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 571);
			}
			else if (Main.rand.Next(25) == 0 && NPC.CountNPCS(569) < num4)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 569);
			}
			if (Main.rand.Next(7) == 0 && NPC.CountNPCS(578) < num7)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 578);
			}
			else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(573) + NPC.CountNPCS(575) < num5)
			{
				num8 = ((Main.rand.Next(3) == 0) ? NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 575) : NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 573));
			}
			else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(563) < num2)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 563);
			}
			else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
			{
				if (Main.rand.Next(3) == 0)
				{
					num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 557);
				}
				num9 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			}
			break;
		case 7:
			if (Main.rand.Next(20) == 0 && NPC.CountNPCS(571) < num6)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 571);
			}
			else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(569) < num4)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 569);
			}
			else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(563) < num2)
			{
				num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 563);
			}
			else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
			{
				if (Main.rand.Next(5) == 0)
				{
					num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 557);
				}
				num9 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			}
			break;
		default:
			num8 = NPC.NewNPC(GetSpawnSource_OldOnesArmy(), x, y, 554);
			break;
		}
		if (Main.netMode == 2 && num8 < Main.maxNPCs)
		{
			NetMessage.SendData(23, -1, -1, null, num8);
		}
		if (Main.netMode == 2 && num9 < Main.maxNPCs)
		{
			NetMessage.SendData(23, -1, -1, null, num9);
		}
	}

	public static bool IsStandActive(int x, int y)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 target = new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8));
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC != null && nPC.active && nPC.type == 548)
			{
				return nPC.Bottom.Distance(target) < 36f;
			}
		}
		return false;
	}

	public static void RequestToSkipWaitTime(int x, int y)
	{
		if (TimeLeftBetweenWaves > 60 && IsStandActive(x, y))
		{
			SoundEngine.PlaySound(SoundID.NPCDeath7, x * 16 + 8, y * 16 + 8);
			if (Main.netMode == 0)
			{
				AttemptToSkipWaitTime();
			}
			else if (Main.netMode != 2)
			{
				NetMessage.SendData(143);
			}
		}
	}

	public static void AttemptToSkipWaitTime()
	{
		if (Main.netMode != 1 && TimeLeftBetweenWaves > 60)
		{
			SetEnemySpawningOnHold(60);
		}
	}

	private static IEntitySource GetSpawnSource_OldOnesArmy()
	{
		return new EntitySource_OldOnesArmy();
	}

	static DD2Event()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
	}
}
