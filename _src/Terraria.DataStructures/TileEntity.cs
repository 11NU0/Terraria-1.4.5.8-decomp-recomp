using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.DataStructures;

public abstract class TileEntity
{
	public static TileEntitiesManager manager;

	public const int MaxEntitiesPerChunk = 1000;

	public static object EntityCreationLock = new object();

	public static List<TileEntity> UpdateEntities = new List<TileEntity>();

	public static Dictionary<int, TileEntity> ByID = new Dictionary<int, TileEntity>();

	public static Dictionary<Point16, TileEntity> ByPosition = new Dictionary<Point16, TileEntity>();

	public static int TileEntitiesNextID;

	public int ID;

	public Point16 Position;

	public byte type;

	public bool RequiresUpdates;

	public static event Action _UpdateStart;

	public static event Action _UpdateEnd;

	public static int AssignNewID()
	{
		return TileEntitiesNextID++;
	}

	public static void Clear()
	{
		ByID.Clear();
		ByPosition.Clear();
		UpdateEntities.Clear();
		TileEntitiesNextID = 0;
	}

	public static void PerformUpdates()
	{
		UpdateStart();
		foreach (TileEntity updateEntity in UpdateEntities)
		{
			updateEntity.Update();
		}
		UpdateEnd();
	}

	private static void UpdateStart()
	{
		if (_UpdateStart != null)
		{
			_UpdateStart();
		}
	}

	private static void UpdateEnd()
	{
		if (_UpdateEnd != null)
		{
			_UpdateEnd();
		}
	}

	public static void Add(TileEntity ent)
	{
		lock (EntityCreationLock)
		{
			ByID[ent.ID] = ent;
			ByPosition[ent.Position] = ent;
			if (ent.RequiresUpdates)
			{
				UpdateEntities.Add(ent);
			}
		}
	}

	public virtual void OnPlaced()
	{
	}

	public virtual void OnRemoved()
	{
	}

	protected static int Place(int x, int y, int type)
	{
		TileEntity tileEntity = manager.GenerateInstance(type);
		tileEntity.Position = new Point16(x, y);
		tileEntity.ID = AssignNewID();
		tileEntity.type = (byte)type;
		Add(tileEntity);
		tileEntity.OnPlaced();
		return tileEntity.ID;
	}

	public static void Kill(int x, int y, int type)
	{
		if (ByPosition.TryGetValue(new Point16(x, y), out var value) && value.type == type)
		{
			Remove(value);
		}
	}

	public static void Remove(TileEntity entity, bool ignorePosition = false)
	{
		lock (EntityCreationLock)
		{
			if (entity.RequiresUpdates)
			{
				UpdateEntities.Remove(entity);
			}
			ByID.Remove(entity.ID);
			if (!ignorePosition)
			{
				ByPosition.Remove(entity.Position);
			}
		}
		entity.OnRemoved();
	}

	public static void InitializeAll()
	{
		manager = new TileEntitiesManager();
		manager.RegisterAll();
	}

	public static void PlaceEntityNet(int x, int y, int type)
	{
		if (WorldGen.InWorld(x, y) && !ByPosition.ContainsKey(new Point16(x, y)))
		{
			manager.NetPlaceEntity(type, x, y);
		}
	}

	public static bool TryGetAt<T>(int x, int y, out T result) where T : TileEntity
	{
		result = null;
		if (ByPosition.TryGetValue(new Point16(x, y), out var value))
		{
			result = value as T;
		}
		return result != null;
	}

	public static bool TryGet<T>(int id, out T result) where T : TileEntity
	{
		result = null;
		if (ByID.TryGetValue(id, out var value))
		{
			result = value as T;
		}
		return result != null;
	}

	public virtual void Update()
	{
	}

	public static void Write(BinaryWriter writer, TileEntity ent, bool networkSend = false)
	{
		writer.Write(ent.type);
		ent.WriteInner(writer, networkSend);
	}

	public static TileEntity Read(BinaryReader reader, int gameVersion, bool networkSend = false)
	{
		byte id = reader.ReadByte();
		TileEntity tileEntity = manager.GenerateInstance(id);
		tileEntity.type = id;
		tileEntity.ReadInner(reader, gameVersion, networkSend);
		return tileEntity;
	}

	private void WriteInner(BinaryWriter writer, bool networkSend)
	{
		if (!networkSend)
		{
			writer.Write(ID);
		}
		writer.Write(Position.X);
		writer.Write(Position.Y);
		WriteExtraData(writer, networkSend);
	}

	private void ReadInner(BinaryReader reader, int gameVersion, bool networkSend)
	{
		if (!networkSend)
		{
			ID = reader.ReadInt32();
		}
		Position = new Point16(reader.ReadInt16(), reader.ReadInt16());
		ReadExtraData(reader, gameVersion, networkSend);
	}

	public virtual void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
	}

	public virtual void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
	{
	}

	public virtual void OnPlayerUpdate(Player player)
	{
	}

	public static bool IsOccupied(int id, out int interactingPlayer)
	{
		interactingPlayer = -1;
		for (int i = 0; i < 255; i++)
		{
			Player player = Main.player[i];
			if (player.active && !player.dead && player.tileEntityAnchor.interactEntityID == id)
			{
				interactingPlayer = i;
				return true;
			}
		}
		return false;
	}

	public virtual void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
	{
	}

	public virtual ItemSlot.AlternateClickAction? GetShiftClickAction(Item[] inv, int context = 0, int slot = 0)
	{
		return null;
	}

	public virtual bool PerformShiftClickAction(Item[] inv, int context = 0, int slot = 0)
	{
		return false;
	}

	public static void BasicOpenCloseInteraction(Player player, int x, int y, int id)
	{
		player.CloseSign();
		int interactingPlayer;
		if (Main.netMode != 1)
		{
			Main.stackSplit = 600;
			player.GamepadEnableGrappleCooldown();
			if (IsOccupied(id, out interactingPlayer))
			{
				if (interactingPlayer == player.whoAmI)
				{
					SoundEngine.PlaySound(11);
					player.tileEntityAnchor.Clear();
				}
			}
			else
			{
				SetInteractionAnchor(player, x, y, id);
			}
			return;
		}
		Main.stackSplit = 600;
		player.GamepadEnableGrappleCooldown();
		if (IsOccupied(id, out interactingPlayer))
		{
			if (interactingPlayer == player.whoAmI)
			{
				SoundEngine.PlaySound(11);
				player.tileEntityAnchor.Clear();
				NetMessage.SendData(122, -1, -1, null, -1, Main.myPlayer);
			}
		}
		else
		{
			NetMessage.SendData(122, -1, -1, null, id, Main.myPlayer);
		}
	}

	public static void SetInteractionAnchor(Player player, int x, int y, int id)
	{
		player.chest = -1;
		player.SetTalkNPC(-1);
		if (player.whoAmI == Main.myPlayer)
		{
			bool flag = player.tileEntityAnchor.interactEntityID == -1;
			IngameUIWindows.CloseAll(quiet: true);
			Main.playerInventory = true;
			Main.PipsUseGrid = false;
			if (PlayerInput.GrappleAndInteractAreShared)
			{
				PlayerInput.Triggers.JustPressed.Grapple = false;
			}
			if (!flag)
			{
				SoundEngine.PlaySound(12);
			}
			else
			{
				SoundEngine.PlaySound(10);
			}
		}
		player.tileEntityAnchor.Set(id, x, y);
	}

	public virtual void RegisterTileEntityID(int assignedID)
	{
	}

	public virtual void NetPlaceEntityAttempt(int x, int y)
	{
	}

	public virtual bool IsTileValidForEntity(int x, int y)
	{
		return false;
	}

	public virtual TileEntity GenerateInstance()
	{
		return null;
	}

	public virtual void OnWorldLoaded()
	{
	}
}
