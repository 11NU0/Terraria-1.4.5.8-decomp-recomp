using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.LeashedEntities;
using Terraria.Net;
using Terraria.Testing;

namespace Terraria.GameContent;

public class LeashedEntity
{
	public class NetModule : Terraria.Net.NetModule
	{
		private enum MessageType
		{
			Remove,
			FullSync,
			PartialSync
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (Main.netMode == 2)
			{
				return false;
			}
			MessageType messageType = (MessageType)reader.ReadByte();
			int slot = reader.Read7BitEncodedInt();
			switch (messageType)
			{
			case MessageType.Remove:
				HandleRemove(slot);
				break;
			case MessageType.FullSync:
				HandleFullSync(slot, reader.Read7BitEncodedInt(), new Point16(reader.ReadInt16(), reader.ReadInt16()), reader);
				break;
			case MessageType.PartialSync:
				HandlePartialSync(slot, reader.Read7BitEncodedInt(), reader);
				break;
			default:
				return false;
			}
			return true;
		}

		public static void Remove(int slot)
		{
			NetPacket packet = Terraria.Net.NetModule.CreatePacket<NetModule>();
			packet.Writer.Write((byte)0);
			packet.Writer.Write7BitEncodedInt(slot);
			NetManager.Instance.Broadcast(packet);
		}

		public static void Sync(LeashedEntity entity, bool full, int toClient = -1)
		{
			NetPacket packet = Terraria.Net.NetModule.CreatePacket<NetModule>();
			packet.Writer.Write((byte)(full ? 1u : 2u));
			packet.Writer.Write7BitEncodedInt(entity.whoAmI);
			packet.Writer.Write7BitEncodedInt(entity.Type);
			if (full)
			{
				packet.Writer.Write(entity.AnchorPosition.X);
				packet.Writer.Write(entity.AnchorPosition.Y);
			}
			entity.NetSend(packet.Writer, full);
			if (toClient >= 0)
			{
				NetManager.Instance.SendToClient(packet, toClient);
				return;
			}
			NetManager.Instance.Broadcast(packet, (int i) =>
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				return Netplay.Clients[i].IsSectionActive(entity.SectionCoordinates);
			});
		}

		private void HandleRemove(int slot)
		{
			if (TryGet(slot, out var entity))
			{
				LeashedEntity.Remove(entity);
			}
		}

		private static void HandleFullSync(int slot, int type, Point16 anchorPos, BinaryReader reader)
		{
			while (slot >= ByWhoAmI.Count)
			{
				ByWhoAmI.Add(null);
			}
			LeashedEntity leashedEntity = ByWhoAmI[slot];
			if (leashedEntity == null)
			{
				leashedEntity = Registry.Get(type).NewInstance();
				AddNewEntity(leashedEntity, anchorPos, slot);
			}
			else if (leashedEntity.Type != type || leashedEntity.AnchorPosition != anchorPos)
			{
				throw new Exception(string.Concat("LeashedEntity type mismatch for full sync. Slot: ", slot, " Existing: ", leashedEntity.Type, " @ ", leashedEntity.AnchorPosition, " New: ", type, " @ ", anchorPos));
			}
			leashedEntity.NetReceive(reader, full: true);
		}

		private static void HandlePartialSync(int slot, int type, BinaryReader reader)
		{
			LeashedEntity leashedEntity = ByWhoAmI[slot];
			if (leashedEntity.Type != type)
			{
				throw new Exception("LeashedEntity type mismatch for full sync. Slot: " + slot + " Existing: " + leashedEntity.Type + " Synced: " + type);
			}
			leashedEntity.NetReceive(reader, full: false);
		}
	}

	public class Registry
	{
		private static readonly List<LeashedEntity> Prototypes = new List<LeashedEntity>();

		public static void RegisterAll()
		{
			Prototypes.Add(null);
			LeashedKite.Prototype = Register<LeashedKite>();
			Register(WalkerLeashedCritter.Prototype);
			Register(AquaticWalkerLeashedCritter.Prototype);
			Register(CrawlerLeashedCritter.Prototype);
			Register(SnailLeashedCritter.Prototype);
			Register(TurtleLeashedCritter.Prototype);
			Register(RunnerLeashedCritter.Prototype);
			Register(FlyerLeashedCritter.Prototype);
			Register(NormalButterflyLeashedCritter.Prototype);
			Register(EmpressButterflyLeashedCritter.Prototype);
			Register(HellButterflyLeashedCritter.Prototype);
			Register(FireflyLeashedCritter.Prototype);
			Register(ShimmerFlyLeashedCritter.Prototype);
			Register(DragonflyLeashedCritter.Prototype);
			Register(CrawlingFlyLeashedCritter.Prototype);
			Register(BirdLeashedCritter.Prototype);
			Register(WaterfowlLeashedCritter.Prototype);
			Register(FishLeashedCritter.Prototype);
			Register(FairyLeashedCritter.Prototype);
			Register(JumperLeashedCritter.Prototype);
			Register(WaterStriderLeashedCritter.Prototype);
		}

		public static void Register(LeashedEntity prototype)
		{
			prototype.Type = Prototypes.Count;
			Prototypes.Add(prototype);
		}

		public static T Register<T>() where T : LeashedEntity, new()
		{
			T val = new T
			{
				Type = Prototypes.Count
			};
			Prototypes.Add(val);
			return val;
		}

		public static LeashedEntity Get(int type)
		{
			return Prototypes[type];
		}
	}

	private class SectionEntityList
	{
		public readonly Point coordinates;

		public bool active;

		public LeashedEntity[] list = new LeashedEntity[32];

		public int count;

		private int emptySlots;

		public SectionEntityList(Point coordinates)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			this.coordinates = coordinates;
		}

		public void Add(LeashedEntity e)
		{
			if (count == list.Length)
			{
				Array.Resize(ref list, list.Length * 2);
			}
			e.sectionSlot = count;
			list[count++] = e;
		}

		public void Remove(LeashedEntity e)
		{
			Invariant.Assert(list[e.sectionSlot] == e, "LeashedEntity.Remove, entity not in sectionSlot");
			list[e.sectionSlot] = null;
			emptySlots++;
		}

		public void CompactIfNecesary()
		{
			if (emptySlots < count / 2)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				LeashedEntity leashedEntity = list[i];
				if (leashedEntity != null)
				{
					leashedEntity.sectionSlot = num;
					list[num++] = leashedEntity;
				}
			}
			Array.Clear(list, num, count - num);
			count = num;
			emptySlots = 0;
		}

		public void Activate()
		{
			if (active)
			{
				return;
			}
			active = true;
			if (Main.netMode != 1)
			{
				LeashedEntity[] array = list;
				foreach (LeashedEntity leashedEntity in array)
				{
					if (leashedEntity != null && leashedEntity.active)
					{
						leashedEntity.Spawn(newlyAdded: false);
					}
				}
			}
			ActiveSectionList.Add(this);
		}

		public void Deactivate()
		{
			active = false;
			if (Main.netMode == 1)
			{
				return;
			}
			LeashedEntity[] array = list;
			foreach (LeashedEntity leashedEntity in array)
			{
				if (leashedEntity != null && leashedEntity.active)
				{
					leashedEntity.Despawn();
				}
			}
		}

		public void Sync(int toClient)
		{
			Invariant.Assert(active, "Syncing an inactive section. Entities may not have spawned");
			LeashedEntity[] array = list;
			foreach (LeashedEntity leashedEntity in array)
			{
				if (leashedEntity != null)
				{
					NetModule.Sync(leashedEntity, full: true, toClient);
				}
			}
		}
	}

	private static readonly SectionEntityList[,] BySection;

	private static readonly List<SectionEntityList> ActiveSectionList;

	private static readonly List<LeashedEntity> ByWhoAmI;

	private int sectionSlot;

	public bool active;

	public int whoAmI;

	public Vector2 position;

	public Vector2 velocity;

	public int direction;

	public int width;

	public int height;

	private const int StreamingRate = 1024;

	public int Type { get; private set; }

	public Point16 AnchorPosition { get; private set; }

	public Point SectionCoordinates
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			return new Point(Netplay.GetSectionX(AnchorPosition.X), Netplay.GetSectionY(AnchorPosition.Y));
		}
	}

	public Vector2 Center
	{
		get
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)(width / 2), position.Y + (float)(height / 2));
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)(width / 2), value.Y - (float)(height / 2));
		}
	}

	public Vector2 Size
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)width, (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			width = (int)value.X;
			height = (int)value.Y;
		}
	}

	static LeashedEntity()
	{
		BySection = new SectionEntityList[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];
		ActiveSectionList = new List<SectionEntityList>();
		ByWhoAmI = new List<LeashedEntity>();
		ActiveSections.SectionActivated += (Point sectionCoordinates) =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			GetSection(sectionCoordinates).Activate();
		};
		RemoteClient.NetSectionActivated += SyncEntitiesInSection;
	}

	internal static void AddGameplaySnapshotComponents()
	{
		StateSnapshot.Gameplay.AddCollection("LeashedEntity.BySection", BySection);
		StateSnapshot.Gameplay.AddCollection("LeashedEntity.ActiveSectionList", ActiveSectionList);
		StateSnapshot.Gameplay.AddCollection("LeashedEntity.ByWhoAmI", ByWhoAmI);
	}

	public static void Clear(bool keepActiveSections = false)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		Array.Clear(BySection, 0, BySection.Length);
		ByWhoAmI.Clear();
		ByWhoAmI.Capacity = 10000;
		ActiveSectionList.Clear();
		ActiveSectionList.Capacity = BySection.Length;
		if (!keepActiveSections)
		{
			return;
		}
		for (int i = 0; i < BySection.GetLength(0); i++)
		{
			for (int j = 0; j < BySection.GetLength(1); j++)
			{
				if (ActiveSections.IsSectionActive(new Point(i, j)))
				{
					GetSection(new Point(i, j)).Activate();
				}
			}
		}
	}

	public static void AddNewEntity(LeashedEntity e, Point16 anchorPos)
	{
		if (e == null)
		{
			return;
		}
		if (Main.netMode == 1)
		{
			Invariant.Assert(condition: false, "AddNewEntity cannot be called on the client");
			return;
		}
		int num = ByWhoAmI.IndexOf(null);
		if (num < 0)
		{
			num = ByWhoAmI.Count;
			ByWhoAmI.Add(null);
		}
		AddNewEntity(e, anchorPos, num);
	}

	private static void AddNewEntity(LeashedEntity e, Point16 anchorPos, int slot)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Invariant.Assert(e.Type != 0, "Type not set for {0}", e);
		e.AnchorPosition = anchorPos;
		e.active = true;
		e.whoAmI = slot;
		ByWhoAmI[slot] = e;
		SectionEntityList section = GetSection(e.SectionCoordinates);
		section.Add(e);
		if (Main.netMode != 1 && section.active)
		{
			e.Spawn(newlyAdded: true);
		}
		if (Main.netMode == 2)
		{
			NetModule.Sync(e, full: true);
		}
	}

	private static SectionEntityList GetSection(Point sectionCoordinates)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		SectionEntityList sectionEntityList = BySection[sectionCoordinates.X, sectionCoordinates.Y];
		if (sectionEntityList == null)
		{
			sectionEntityList = (BySection[sectionCoordinates.X, sectionCoordinates.Y] = new SectionEntityList(sectionCoordinates));
		}
		return sectionEntityList;
	}

	private static void Remove(LeashedEntity e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		e.active = false;
		ByWhoAmI[e.whoAmI] = null;
		while (ByWhoAmI.Count > 0 && ByWhoAmI[ByWhoAmI.Count - 1] == null)
		{
			ByWhoAmI.RemoveAt(ByWhoAmI.Count - 1);
		}
		GetSection(e.SectionCoordinates).Remove(e);
		if (Main.netMode == 2)
		{
			NetModule.Remove(e.whoAmI);
		}
	}

	public static bool TryGet(int slot, out LeashedEntity entity)
	{
		entity = null;
		if (slot < 0 || slot >= ByWhoAmI.Count)
		{
			return false;
		}
		entity = ByWhoAmI[slot];
		return entity != null;
	}

	public static void UpdateEntities()
	{
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		RecheckActiveSections();
		_UpdateEntities();
		TimeLogger.UpdateLeashedEntities.AddTime(fromTimestamp);
	}

	private static void RecheckActiveSections()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < ActiveSectionList.Count; i++)
		{
			SectionEntityList sectionEntityList = ActiveSectionList[i];
			Invariant.Assert(sectionEntityList.active, "Inactive section in ActiveSectionList");
			sectionEntityList.CompactIfNecesary();
			if (!ActiveSections.IsSectionActive(sectionEntityList.coordinates))
			{
				sectionEntityList.Deactivate();
			}
			else
			{
				ActiveSectionList[num++] = sectionEntityList;
			}
		}
		ActiveSectionList.RemoveRange(num, ActiveSectionList.Count - num);
	}

	private static void _UpdateEntities()
	{
		foreach (SectionEntityList activeSection in ActiveSectionList)
		{
			LeashedEntity[] list = activeSection.list;
			int count = activeSection.count;
			for (int i = 0; i < count; i++)
			{
				LeashedEntity leashedEntity = list[i];
				if (leashedEntity != null)
				{
					if (leashedEntity.active)
					{
						leashedEntity.Update();
						leashedEntity.StreamNetUpdates();
					}
					if (!leashedEntity.active)
					{
						Remove(leashedEntity);
					}
				}
			}
		}
	}

	private void StreamNetUpdates()
	{
		if (Main.netMode == 2 && ((Main.GameUpdateCount + whoAmI) & 0x3FF) == 0L)
		{
			NetModule.Sync(this, full: false);
		}
	}

	private static void SyncEntitiesInSection(int toClient, Point sectionCoordinates)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		GetSection(sectionCoordinates).Sync(toClient);
	}

	public static void DrawEntities()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		Rectangle val = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
		val.Inflate(512, 512);
		foreach (SectionEntityList activeSection in ActiveSectionList)
		{
			LeashedEntity[] list = activeSection.list;
			int count = activeSection.count;
			for (int i = 0; i < count; i++)
			{
				LeashedEntity leashedEntity = list[i];
				if (leashedEntity != null && val.Contains(leashedEntity.Center.ToPoint()))
				{
					leashedEntity.Draw();
				}
			}
		}
		TimeLogger.LeashedEntities.AddTime(fromTimestamp);
	}

	public virtual LeashedEntity NewInstance()
	{
		LeashedEntity leashedEntity = (LeashedEntity)Activator.CreateInstance(GetType(), nonPublic: true);
		leashedEntity.Type = Type;
		return leashedEntity;
	}

	public virtual void Spawn(bool newlyAdded)
	{
	}

	public virtual void Despawn()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void Draw()
	{
	}

	public virtual void NetSend(BinaryWriter writer, bool full)
	{
	}

	public virtual void NetReceive(BinaryReader reader, bool full)
	{
	}

	public bool NearbySectionsMissing(int fluff = 3)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 1)
		{
			return false;
		}
		Point val = position.ToTileCoordinates().ClampedInWorld(fluff);
		if (Main.tile[val.X - fluff, val.Y] != null && Main.tile[val.X + fluff, val.Y] != null && Main.tile[val.X, val.Y - fluff] != null)
		{
			return Main.tile[val.X, val.Y + fluff] == null;
		}
		return true;
	}
}
