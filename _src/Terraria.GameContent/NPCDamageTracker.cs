using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Testing;

namespace Terraria.GameContent;

public abstract class NPCDamageTracker
{
	public class CustomDefinition
	{
		public List<int> NPCTypes;

		public LocalizedText Name;
	}

	protected abstract class CreditEntry : IComparable<CreditEntry>
	{
		public int Damage { get; set; }

		public int HitCount { get; set; }

		public abstract NetworkText Name { get; }

		public int CompareTo(CreditEntry other)
		{
			int num = 0;
			if (num == 0)
			{
				num = -Damage.CompareTo(other.Damage);
			}
			if (num == 0)
			{
				num = -HitCount.CompareTo(other.HitCount);
			}
			return num;
		}
	}

	private class PlayerCreditEntry : CreditEntry
	{
		public readonly string PlayerName;

		public readonly PlayerNPCHitSource Source;

		public override NetworkText Name
		{
			get
			{
				string text = PlayerName;
				if (Source != default(PlayerNPCHitSource))
				{
					text = text + " - " + Source.Name;
				}
				return NetworkText.FromLiteral(text);
			}
		}

		public PlayerCreditEntry(string name, PlayerNPCHitSource source)
		{
			PlayerName = name;
			Source = source;
		}
	}

	private class WorldCreditEntry : CreditEntry
	{
		public override NetworkText Name => NetworkText.FromKey("BossDamageCommand.WorldCreditName");
	}

	public static CustomDefinition[] CustomBossDefinitions;

	public static int[] BossTypeForMob;

	private static List<NPCDamageTracker> _activeTrackers;

	private static List<NPCDamageTracker> _recentFinishedTrackers;

	private static readonly int MAX_RECENT_TRACKERS;

	private static readonly int EXTRA_RECENT_TRACKER_EXPIRY_TIME;

	private static PlayerNPCHitSource _nextHitSource;

	public static bool UseExpandedSources;

	private readonly List<CreditEntry> _list = new List<CreditEntry>(255);

	private WorldCreditEntry _worldCredit;

	private CreditEntry _lastAttacker;

	private int _ticks;

	private int _lastHitTime;

	public bool IsEmpty => _list.Count == 0;

	public int Duration => _lastHitTime;

	public int TimeSinceLastHit => _ticks - _lastHitTime;

	public abstract LocalizedText Name { get; }

	public abstract LocalizedText KillTimeMessage { get; }

	public static CustomDefinition RegisterCompositeTypeBoss(params int[] types)
	{
		CustomDefinition customDefinition = new CustomDefinition
		{
			NPCTypes = types.ToList()
		};
		foreach (int num in types)
		{
			CustomBossDefinitions[num] = customDefinition;
		}
		return customDefinition;
	}

	public static void RegisterMobsForBoss(int bossType, params int[] mobTypes)
	{
		foreach (int num in mobTypes)
		{
			BossTypeForMob[num] = bossType;
		}
	}

	static NPCDamageTracker()
	{
		CustomBossDefinitions = NPCID.Sets.Factory.CreateCustomSet<CustomDefinition>(null, new object[0]);
		BossTypeForMob = NPCID.Sets.Factory.CreateIntSet();
		_activeTrackers = new List<NPCDamageTracker>();
		_recentFinishedTrackers = new List<NPCDamageTracker>();
		MAX_RECENT_TRACKERS = 3;
		EXTRA_RECENT_TRACKER_EXPIRY_TIME = 54000;
		UseExpandedSources = false;
		RegisterMobsForBoss(50, 1, 535);
		RegisterMobsForBoss(4, 5);
		RegisterMobsForBoss(222, 210, 211);
		RegisterCompositeTypeBoss(13, 14, 15);
		RegisterCompositeTypeBoss(266, 267);
		RegisterCompositeTypeBoss(35, 36);
		RegisterMobsForBoss(113, 115, 116, 117, 118, 119);
		RegisterMobsForBoss(657, 658, 659, 660);
		RegisterCompositeTypeBoss(126, 125).Name = Language.GetText("Enemies.TheTwins");
		RegisterCompositeTypeBoss(127, 128, 129, 130, 131);
		RegisterMobsForBoss(134, 139);
		RegisterMobsForBoss(262, 264);
		RegisterCompositeTypeBoss(245, 246, 247, 248);
		RegisterMobsForBoss(370, 372, 373);
		RegisterMobsForBoss(439, 454, 455, 456, 457, 458, 459);
		RegisterCompositeTypeBoss(398, 396, 397).Name = Language.GetText("Enemies.MoonLord");
	}

	internal static void AddGameplaySnapshotComponents()
	{
		StateSnapshot.Gameplay.AddRef("NPCDamageTracker._activeTrackers", () => _activeTrackers, (List<NPCDamageTracker> v) =>
		{
			_activeTrackers = v;
		});
		StateSnapshot.Gameplay.AddRef("NPCDamageTracker._recentFinishedTrackers", () => _recentFinishedTrackers, (List<NPCDamageTracker> v) =>
		{
			_recentFinishedTrackers = v;
		});
	}

	private static bool GetRealActiveNPC(ref NPC npc)
	{
		if (npc.realLife >= 0)
		{
			npc = Main.npc[npc.realLife];
		}
		if (!npc.active)
		{
			return false;
		}
		return true;
	}

	private static bool TryGetTrackerFor(NPC npc, out NPCDamageTracker tracker)
	{
		tracker = null;
		if (!GetRealActiveNPC(ref npc))
		{
			return false;
		}
		foreach (NPCDamageTracker activeTracker in _activeTrackers)
		{
			if (activeTracker.IncludeDamageFor(npc))
			{
				tracker = activeTracker;
				return true;
			}
		}
		return false;
	}

	private static bool CreateTrackerFor(NPC npc, out NPCDamageTracker tracker)
	{
		tracker = null;
		CustomDefinition customDefinition = CustomBossDefinitions[npc.type];
		if (customDefinition == null && !npc.boss)
		{
			return false;
		}
		tracker = new BossDamageTracker(npc.type, customDefinition);
		return true;
	}

	public static void AddDamage(NPC npc, int owner, int damage)
	{
		if (!GetRealActiveNPC(ref npc) || npc.life <= 0)
		{
			return;
		}
		if (!TryGetTrackerFor(npc, out var tracker))
		{
			if (!CreateTrackerFor(npc, out tracker))
			{
				return;
			}
			Start(tracker);
		}
		tracker.AddDamage(owner, Math.Min(damage, npc.life));
	}

	public static void AddDamageToLastAttack(NPC npc, int damage)
	{
		if (GetRealActiveNPC(ref npc) && npc.life > 0 && TryGetTrackerFor(npc, out var tracker))
		{
			tracker.AddDamageToLastAttack(Math.Min(damage, npc.life));
		}
	}

	public static void SetSourceForNextHit(NPC npc, PlayerNPCHitSource source)
	{
		_nextHitSource = source;
	}

	public static void BossKilled(NPC npc)
	{
		if (TryGetTrackerFor(npc, out var tracker))
		{
			tracker.OnBossKilled(npc);
		}
	}

	public static void Start(NPCDamageTracker tracker)
	{
		_activeTrackers.Add(tracker);
	}

	public static void Reset()
	{
		_activeTrackers.Clear();
		_recentFinishedTrackers.Clear();
	}

	public static IEnumerable<NPCDamageTracker> RecentAttempts()
	{
		return _recentFinishedTrackers;
	}

	public static void Update()
	{
		foreach (NPCDamageTracker activeTracker in _activeTrackers)
		{
			activeTracker.Tick();
		}
		foreach (NPCDamageTracker recentFinishedTracker in _recentFinishedTrackers)
		{
			recentFinishedTracker.Tick();
		}
		for (int num = _activeTrackers.Count - 1; num >= 0; num--)
		{
			_activeTrackers[num].CheckActive();
		}
		while (_recentFinishedTrackers.Count > 1 && _recentFinishedTrackers[0].TimeSinceLastHit > EXTRA_RECENT_TRACKER_EXPIRY_TIME)
		{
			_recentFinishedTrackers.RemoveAt(0);
		}
	}

	private static void StopTracking(NPCDamageTracker tracker)
	{
		if (_activeTrackers.Remove(tracker) && !tracker.IsEmpty)
		{
			_recentFinishedTrackers.Add(tracker);
			if (_recentFinishedTrackers.Count > MAX_RECENT_TRACKERS)
			{
				_recentFinishedTrackers.RemoveAt(0);
			}
		}
	}

	protected abstract bool IncludeDamageFor(NPC npc);

	protected virtual void CheckActive()
	{
	}

	protected virtual void OnBossKilled(NPC npc)
	{
	}

	private void Tick()
	{
		_ticks++;
	}

	protected void Stop()
	{
		StopTracking(this);
	}

	public void AddDamage(int owner, int damage)
	{
		_lastHitTime = _ticks;
		CreditEntry orAddEntry = GetOrAddEntry(owner);
		orAddEntry.Damage += damage;
		orAddEntry.HitCount++;
		_lastAttacker = orAddEntry;
	}

	public void AddDamageToLastAttack(int damage)
	{
		_lastAttacker.Damage += damage;
	}

	protected virtual CreditEntry GetOrAddEntry(int owner)
	{
		PlayerNPCHitSource playerNPCHitSource = _nextHitSource;
		_nextHitSource = default;
		if (!UseExpandedSources)
		{
			playerNPCHitSource = default;
		}
		if (owner < 0 || owner >= 255)
		{
			if (_worldCredit == null)
			{
				_worldCredit = new WorldCreditEntry();
				_list.Add(_worldCredit);
			}
			return _worldCredit;
		}
		string name = Main.player[owner].name;
		foreach (CreditEntry item in _list)
		{
			if (item is PlayerCreditEntry playerCreditEntry && playerCreditEntry.PlayerName == name && playerCreditEntry.Source == playerNPCHitSource)
			{
				return playerCreditEntry;
			}
		}
		PlayerCreditEntry playerCreditEntry2 = new PlayerCreditEntry(name, playerNPCHitSource);
		_list.Add(playerCreditEntry2);
		return playerCreditEntry2;
	}

	public NetworkText GetReport(Player forPlayer = null)
	{
		_list.Sort();
		int[] array = _list.Select((CreditEntry x) => x.Damage).ToArray();
		int[] array2 = CalculatePercentages(array);
		int length = array.Max().ToString().Length;
		List<NetworkText> list = new List<NetworkText>(_list.Count + 2);
		StringBuilder stringBuilder = new StringBuilder("{0}");
		list.Add(NetworkText.FromKey("BossDamageCommand.Title", Name.ToNetworkText()));
		LocalizedText killTimeMessage = KillTimeMessage;
		if (killTimeMessage != null)
		{
			stringBuilder.Append("\n{1}");
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)Duration / 60.0);
			string text = $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:00}";
			list.Add(killTimeMessage.ToNetworkText(text));
		}
		for (int num = 0; num < _list.Count; num++)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(array2[num]).Append('%');
			while (stringBuilder2.Length < 6)
			{
				stringBuilder2.Append(' ');
			}
			stringBuilder2.Append(array[num]);
			while (stringBuilder2.Length < 8 + length)
			{
				stringBuilder2.Append(' ');
			}
			CreditEntry creditEntry = _list[num];
			stringBuilder2.Append('{').Append(list.Count).Append('}');
			list.Add(creditEntry.Name);
			if (UseExpandedSources)
			{
				stringBuilder2.Append(" - ").Append(creditEntry.HitCount).Append(" hits");
			}
			string text2 = stringBuilder2.ToString();
			if (forPlayer != null && creditEntry is PlayerCreditEntry && ((PlayerCreditEntry)creditEntry).PlayerName == forPlayer.name)
			{
				text2 = "[c/FFAF00:" + text2 + "]";
			}
			stringBuilder.Append('\n').Append(text2);
		}
		string text3 = stringBuilder.ToString();
		object[] substitutions = list.ToArray();
		return NetworkText.FromFormattable(text3, substitutions);
	}

	public static int[] CalculatePercentages(int[] damages)
	{
		int num = damages.Sum();
		int[] array = new int[damages.Length];
		double[] array2 = new double[damages.Length];
		int i = 0;
		for (int j = 0; j < damages.Length; j++)
		{
			double num2 = (double)(damages[j] * 100) / (double)num;
			int num3 = (array[j] = (int)num2);
			array2[j] = num2 - (double)num3;
			i += num3;
		}
		for (; i < 100; i++)
		{
			int num4 = 0;
			double num5 = 0.0;
			for (int k = 0; k < damages.Length; k++)
			{
				if (array2[k] > num5)
				{
					num5 = array2[k];
					num4 = k;
				}
			}
			array2[num4] = 0.0;
			array[num4]++;
		}
		return array;
	}
}
