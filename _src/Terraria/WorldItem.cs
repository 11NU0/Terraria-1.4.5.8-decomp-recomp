using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Testing;

namespace Terraria;

public class WorldItem : Entity
{
	public int playerIndexTheItemIsReservedFor;

	public int timeSinceTheItemHasBeenReservedForSomeone;

	public int timeToKeepReservation;

	public int grabDelayPlayer;

	public int grabDelayTime;

	public int enemyGrabDelayTime;

	public static readonly int DefaultGrabDelay;

	public bool shimmered;

	public float shimmerTime;

	public bool instanced;

	public int timeSinceItemSpawned;

	public bool beingGrabbed;

	public bool onConveyor;

	public Item inner { get; private set; }

	public bool active => type != 0;

	public int type
	{
		get
		{
			return inner.type;
		}
		set
		{
			inner.type = value;
		}
	}

	public int stack
	{
		get
		{
			return inner.stack;
		}
		set
		{
			inner.stack = value;
		}
	}

	public byte prefix
	{
		get
		{
			return inner.prefix;
		}
		set
		{
			inner.prefix = value;
		}
	}

	public Color color
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return inner.color;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			inner.color = value;
		}
	}

	public int value => inner.value;

	public int maxStack => inner.maxStack;

	public float scale => inner.scale;

	public string Name => inner.Name;

	public bool IsACoin => inner.IsACoin;

	public bool IsAir => inner.IsAir;

	static WorldItem()
	{
		DefaultGrabDelay = 100;
		RemoteClient.NetSectionActivated += SyncItemsInSection;
	}

	public override string ToString()
	{
		return "[" + whoAmI + "]" + inner;
	}

	public void ReplaceWith(Item item)
	{
		if (inner.IsAir && !item.IsAir)
		{
			throw new Exception("Attempt to create a WorldItem manually?");
		}
		inner = item;
		inner.newAndShiny = true;
	}

	public WorldItem()
		: this(new Item())
	{
	}

	public WorldItem(Item item)
	{
		inner = item;
		inner.newAndShiny = true;
		width = (height = 16);
		playerIndexTheItemIsReservedFor = ((Main.netMode == 0) ? Main.myPlayer : 255);
	}

	public void TurnToAir()
	{
		inner.TurnToAir();
	}

	public void Prefix(int prefix)
	{
		inner.Prefix(prefix);
	}

	public bool OnlyNeedOneInInventory()
	{
		return inner.OnlyNeedOneInInventory();
	}

	public void TryCombiningIntoNearbyItems()
	{
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		if (playerIndexTheItemIsReservedFor != Main.myPlayer || shimmerTime > 0f || !inner.CanPassivelyStackInWorld() || stack >= maxStack)
		{
			return;
		}
		int num = 30;
		for (int i = whoAmI + 1; i < 400; i++)
		{
			WorldItem worldItem = Main.item[i];
			if (!worldItem.IsAir && Item.CanStack(inner, worldItem.inner) && worldItem.shimmered == shimmered && worldItem.playerIndexTheItemIsReservedFor == playerIndexTheItemIsReservedFor && !(Math.Abs(position.X - worldItem.position.X) + Math.Abs(position.Y - worldItem.position.Y) > (float)num))
			{
				int num2 = Math.Min(worldItem.stack, maxStack - stack);
				worldItem.stack -= num2;
				stack += num2;
				float num3 = (float)num2 / (float)stack;
				position = Vector2.Lerp(worldItem.position, position, num3);
				velocity = Vector2.Lerp(worldItem.velocity, velocity, num3);
				if (worldItem.stack <= 0)
				{
					worldItem.TurnToAir();
				}
				if (Main.netMode != 0)
				{
					SyncItem();
					worldItem.SyncItem();
				}
			}
		}
	}

	public void FindOwner(bool forceAssignToServer = false)
	{
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		Invariant.Assert(Main.netMode != 0, "FindOwner in singleplayer");
		if (forceAssignToServer)
		{
			timeToKeepReservation = 0;
		}
		if (instanced || timeToKeepReservation > 0)
		{
			return;
		}
		int num = playerIndexTheItemIsReservedFor;
		int num2 = 255;
		if (forceAssignToServer || shimmerTime > 0f || (grabDelayTime > 0 && grabDelayPlayer == 255) || EmergencyStacking.HasPendingTransferInvolving(this))
		{
			num2 = 255;
		}
		else
		{
			float num3 = NPC.sWidth;
			for (int i = 0; i < 255; i++)
			{
				if (grabDelayTime > 0 && grabDelayPlayer == i)
				{
					continue;
				}
				Player player = Main.player[i];
				if (!player.active || player.dead)
				{
					continue;
				}
				Player.ItemSpaceStatus status = player.ItemSpace(this);
				if (player.CanPullItem(this, status))
				{
					float num4 = Math.Abs(player.position.X + (float)(player.width / 2) - position.X - (float)(width / 2)) + Math.Abs(player.position.Y + (float)(player.height / 2) - position.Y - (float)height);
					if (player.manaMagnet && (type == 184 || type == 1735 || type == 1868))
					{
						num4 -= (float)Item.manaGrabRange;
					}
					if (player.lifeMagnet && (type == 58 || type == 1734 || type == 1867))
					{
						num4 -= (float)Item.lifeGrabRange;
					}
					if (type == 4143)
					{
						num4 -= (float)Item.manaGrabRange;
					}
					if (num3 > num4)
					{
						num3 = num4;
						num2 = i;
					}
				}
			}
			if (Main.netMode != 0 && num2 != 255)
			{
				Player obj = Main.player[num2];
				int itemGrabRange = obj.GetItemGrabRange(this);
				Rectangle hitbox = obj.Hitbox;
				hitbox.Inflate(itemGrabRange, itemGrabRange);
				if (!hitbox.Intersects(Hitbox) && Wiring.IsHopperInRangeOf(this))
				{
					num2 = 255;
				}
			}
		}
		if (num2 == num)
		{
			return;
		}
		if (Main.netMode == 1)
		{
			playerIndexTheItemIsReservedFor = 255;
			NetMessage.SendData(39, -1, -1, null, whoAmI, forceAssignToServer ? 1 : 0);
		}
		else if (num != Main.myPlayer && Main.player[num].active)
		{
			playerIndexTheItemIsReservedFor = num;
			if (timeSinceTheItemHasBeenReservedForSomeone >= 0)
			{
				timeSinceTheItemHasBeenReservedForSomeone = -1;
				NetMessage.SendData(39, num, -1, null, whoAmI);
			}
		}
		else
		{
			ReserveFor(num2);
		}
	}

	public void ReserveFor(int player, int timeToKeepReservation = 15)
	{
		Invariant.Assert(Main.netMode == 2, "Can only be called on server");
		Invariant.Assert(player == 255 || Main.player[player].active, "Attempted to reserve for a disconnected player");
		if (playerIndexTheItemIsReservedFor != 255 && Main.player[playerIndexTheItemIsReservedFor].active)
		{
			Invariant.Assert(condition: false, "Already reserved for someone else");
			return;
		}
		playerIndexTheItemIsReservedFor = player;
		timeSinceTheItemHasBeenReservedForSomeone = 0;
		NetMessage.SendData(22, -1, -1, null, whoAmI, timeToKeepReservation);
	}

	public void ApplySpawnOwnership(NewItemOwnership owner, int localPlayerIndex)
	{
		if (Main.netMode == 1)
		{
			return;
		}
		switch (owner)
		{
		case NewItemOwnership.ReserveForLocalPlayer:
			if (Main.netMode == 2 && localPlayerIndex == 255)
			{
				Invariant.Assert(condition: false, "Item spawned on server with ReserveForLocalPlayer but no local player context");
			}
			else if (Main.netMode != 0)
			{
				ReserveFor(localPlayerIndex, DefaultGrabDelay);
				return;
			}
			break;
		case NewItemOwnership.GrabDelayForLocalPlayer:
			grabDelayTime = DefaultGrabDelay;
			grabDelayPlayer = localPlayerIndex;
			break;
		case NewItemOwnership.GrabDelayForAllPlayers:
			grabDelayTime = DefaultGrabDelay;
			grabDelayPlayer = 255;
			break;
		}
		if (Main.netMode != 0)
		{
			FindOwner();
		}
	}

	public void MakeInstanced(Predicate<Player> shouldSpawnForPlayer, int defaultLifetimeOverride = 0)
	{
		if (Main.netMode != 2)
		{
			return;
		}
		Main.timeItemSlotCannotBeReusedFor[whoAmI] = ((defaultLifetimeOverride > 0) ? defaultLifetimeOverride : 54000);
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active && shouldSpawnForPlayer(Main.player[i]))
			{
				NetMessage.SendData(90, i, -1, null, whoAmI);
			}
		}
		TurnToAir();
	}

	public void UpdateItem(int i)
	{
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		whoAmI = i;
		if (Main.timeItemSlotCannotBeReusedFor[i] > 0)
		{
			if (Main.netMode != 2)
			{
				Invariant.Assert(condition: false, "timeItemSlotCannotBeReusedFor set on client?");
				return;
			}
			Main.timeItemSlotCannotBeReusedFor[i]--;
			if (Main.timeItemSlotCannotBeReusedFor[i] == 0)
			{
				NetMessage.SendData(151, -1, -1, null, i);
			}
		}
		if (!active)
		{
			return;
		}
		if (instanced && Main.netMode == 2)
		{
			Invariant.Assert(condition: false, "Instanced item on the server?");
			TurnToAir();
			return;
		}
		float gravity = 0.1f;
		float maxFallSpeed = 7f;
		if (Main.netMode == 1)
		{
			Point val = Bottom.ToTileCoordinates();
			if (!WorldGen.IsTileLoaded(val.X, val.Y))
			{
				gravity = 0f;
				velocity = Vector2.Zero;
				if (instanced && Main.GameUpdateCount % 10 == 0)
				{
					NetMessage.SendData(159, -1, -1, null, val.X / 200, val.Y / 150);
				}
			}
		}
		Vector2 wetVelocity = velocity * 0.5f;
		if (shimmerWet)
		{
			gravity = 0.065f;
			maxFallSpeed = 4f;
			wetVelocity = velocity * 0.375f;
		}
		else if (honeyWet)
		{
			gravity = 0.05f;
			maxFallSpeed = 3f;
			wetVelocity = velocity * 0.25f;
		}
		else if (wet)
		{
			gravity = 0.08f;
			maxFallSpeed = 5f;
		}
		if (!beingGrabbed)
		{
			if (type == 205)
			{
				TryFillBucket();
			}
			UpdateShimmer(ref gravity);
			TryCombiningIntoNearbyItems();
			if (enemyGrabDelayTime == 0 && playerIndexTheItemIsReservedFor == Main.myPlayer)
			{
				GetPickedUpByMonsters_Special();
				if (Main.expertMode && IsACoin)
				{
					GetPickedUpByMonsters_Money();
				}
			}
			MoveInWorld(gravity, maxFallSpeed, ref wetVelocity);
			if (type == 74)
			{
				TryGrantingMakeAWishSet();
			}
			if (lavaWet)
			{
				CheckLavaDeath();
			}
			CheckInWorld();
			DespawnIfMeetingConditions();
		}
		else
		{
			wet = false;
			wetCount = 0;
			lavaWet = false;
			honeyWet = false;
			shimmerWet = false;
			beingGrabbed = false;
			onConveyor = false;
			ApplyMovement(ref wetVelocity);
		}
		UpdateItem_VisualEffects();
		if (timeSinceItemSpawned < 2147483547)
		{
			timeSinceItemSpawned++;
		}
		if (grabDelayTime > 0)
		{
			grabDelayTime--;
		}
		if (timeToKeepReservation > 0)
		{
			timeToKeepReservation--;
		}
		if (enemyGrabDelayTime > 0)
		{
			enemyGrabDelayTime--;
		}
	}

	private void UpdateShimmer(ref float gravity)
	{
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		if (shimmered)
		{
			if (Main.rand.Next(30) == 0)
			{
				int num = Dust.NewDust(position, width, height, 309);
				Main.dust[num].position.X += Main.rand.Next(-8, 5);
				Main.dust[num].position.Y += Main.rand.Next(-8, 5);
				Main.dust[num].scale *= 1.1f;
				Dust obj = Main.dust[num];
				obj.velocity *= 0.3f;
				switch (Main.rand.Next(6))
				{
				case 0:
					Main.dust[num].color = new Color(255, 255, 210);
					break;
				case 1:
					Main.dust[num].color = new Color(190, 245, 255);
					break;
				case 2:
					Main.dust[num].color = new Color(255, 150, 255);
					break;
				default:
					Main.dust[num].color = new Color(190, 175, 255);
					break;
				}
			}
			Lighting.AddLight(Center, (1f - shimmerTime) * 0.8f, (1f - shimmerTime) * 0.8f, (1f - shimmerTime) * 0.8f);
			gravity = 0f;
			if (shimmerWet)
			{
				if (velocity.Y > -4f)
				{
					velocity.Y -= 0.05f;
				}
			}
			else
			{
				int num2 = 2;
				int num3 = (int)(Center.X / 16f);
				int num4 = (int)(Center.Y / 16f);
				bool flag = false;
				for (int i = num4; i < num4 + num2; i++)
				{
					if (WorldGen.InWorld(num3, i) && Main.tile[num3, i] != null && Main.tile[num3, i].shimmer() && Main.tile[num3, i].liquid > 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (velocity.Y > -4f)
					{
						velocity.Y -= 0.05f;
					}
				}
				else
				{
					velocity.Y *= 0.9f;
				}
			}
		}
		if (shimmerWet && !shimmered && CanShimmerAtPosition())
		{
			shimmerTime += 0.01f;
			if (shimmerTime > 1f)
			{
				shimmerTime = 1f;
			}
			if (playerIndexTheItemIsReservedFor == Main.myPlayer)
			{
				if (Main.netMode == 1)
				{
					FindOwner(forceAssignToServer: true);
				}
				else if (shimmerTime > 0.9f)
				{
					shimmerTime = 0.9f;
					GetShimmered();
				}
			}
		}
		else if (shimmerTime > 0f)
		{
			shimmerTime -= 0.01f;
			if (shimmerTime < 0f)
			{
				shimmerTime = 0f;
			}
		}
	}

	private void TryFillBucket()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (playerIndexTheItemIsReservedFor != Main.myPlayer || !Main.raining || (!Main.isThereAWorldSurface && !Main.remixWorld) || !WorldGen.IsSurfaceForAtmospherics(position.ToTileCoordinates()))
		{
			return;
		}
		int num = (int)Center.X / 16;
		int num2 = (int)Center.Y / 16;
		if (WorldGen.InWorld(num, num2) && WallID.Sets.AllowsWind[Main.tile[num, num2].wall])
		{
			int num3 = 600;
			if (Main.dayRate > 0 && Main.dayRate < num3)
			{
				num3 /= Main.dayRate;
			}
			if (Main.rand.Next(num3) == 0 && Main.rand.NextFloat() < Main.maxRaining)
			{
				int num4 = stack;
				inner.SetDefaults(206);
				stack = num4;
				SyncItem();
			}
		}
	}

	private void CheckInWorld()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(position.ToTileCoordinates(), 20))
		{
			if (ItemID.Sets.RecoverableImportantItem[type])
			{
				Point p = (((!instanced && Main.netMode != 0) || Main.LocalPlayer.SpawnX < 0) ? new Point(Main.spawnTileX, Main.spawnTileY) : new Point(Main.LocalPlayer.SpawnX, Main.LocalPlayer.SpawnY));
				Center = p.ToWorldCoordinates();
				velocity = Vector2.Zero;
			}
			else
			{
				TurnToAir();
			}
			SyncItem();
		}
	}

	private void TryGrantingMakeAWishSet()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		if (playerIndexTheItemIsReservedFor == Main.myPlayer && wet && stack == 1 && !shimmerWet && grabDelayTime > 0 && grabDelayPlayer != 255)
		{
			byte b = Player.FindClosest(position, width, height);
			if (b != byte.MaxValue && Main.player[b].ZoneDesert)
			{
				TurnToAirAndSync();
				int num = 0;
				SpawnShimmeredItem(num++, 5655);
				SpawnShimmeredItem(num++, 5656);
				SpawnShimmeredItem(num++, 5657);
				SpawnShimmeredItem(num++, 5658);
				SpawnShimmeredItem(num++, 5661);
				ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.HeroicisSetSpawnSound, new ParticleOrchestraSettings
				{
					PositionInWorld = Center
				});
			}
		}
	}

	public void SyncItem()
	{
		NetMessage.SendData(21, -1, -1, null, whoAmI);
	}

	public void TurnToAirAndSync()
	{
		TurnToAir();
		SyncItem();
	}

	public void SpawnShimmeredItem(int itemNumber, int type, int stack = 1)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		SpawnShimmeredItem(GetItemSource_Misc(ItemSourceID.Shimmer), Center, itemNumber, type, stack);
	}

	public static void SpawnShimmeredItem(IEntitySource source, Vector2 center, int itemNumber, int type, int stack = 1)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)(itemNumber * (itemNumber % 2 * 2 - 1)), 0f);
		Item.RequestNewItem(source, center, type, stack, 0, NewItemOwnership.None, val, (WorldItem item) =>
		{
			item.shimmerTime = 1f;
			item.shimmered = true;
			item.shimmerWet = true;
			item.wet = true;
		});
	}

	private void DespawnIfMeetingConditions()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		if (playerIndexTheItemIsReservedFor != Main.myPlayer)
		{
			return;
		}
		if (type == 75 && Main.dayTime && !Main.remixWorld && !shimmered && !beingGrabbed)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(position, width, height, 15, velocity.X, velocity.Y, 150, default, 1.2f);
			}
			for (int j = 0; j < 3; j++)
			{
				Gore.NewGore(position, new Vector2(velocity.X, velocity.Y), Main.rand.Next(16, 18));
			}
			TurnToAirAndSync();
		}
		if (type == 4143 && timeSinceItemSpawned > 300)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(position, width, height, 15, velocity.X, velocity.Y, 150, Color.Lerp(Color.CornflowerBlue, Color.Indigo, Main.rand.NextFloat()), 1.2f);
			}
			TurnToAirAndSync();
		}
		if (type == 3822 && !DD2Event.Ongoing)
		{
			int num = Main.rand.Next(18, 24);
			for (int l = 0; l < num; l++)
			{
				int num2 = Dust.NewDust(Center, 0, 0, 61, 0f, 0f, 0, default, 1.7f);
				Dust obj = Main.dust[num2];
				obj.velocity *= 8f;
				Main.dust[num2].velocity.Y--;
				Main.dust[num2].position = Vector2.Lerp(Main.dust[num2].position, Center, 0.5f);
				Main.dust[num2].noGravity = true;
				Main.dust[num2].noLight = true;
			}
			TurnToAirAndSync();
		}
	}

	private void CheckLavaDeath()
	{
		if (playerIndexTheItemIsReservedFor != Main.myPlayer || IsAir)
		{
			return;
		}
		if (type == 267)
		{
			VoodooDollLavaDeath();
			return;
		}
		int rare = inner.rare;
		if ((rare == 0 || rare == -1) && !ItemID.Sets.IsLavaImmuneRegardlessOfRarity[type])
		{
			TurnToAirAndSync();
		}
	}

	private void VoodooDollLavaDeath()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			FindOwner(forceAssignToServer: true);
			return;
		}
		int num = stack;
		TurnToAirAndSync();
		bool flag = false;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			if (Main.npc[i].active && Main.npc[i].type == 22)
			{
				int num2 = -Main.npc[i].direction;
				if (Main.npc[i].IsNPCValidForBestiaryKillCredit())
				{
					Main.BestiaryTracker.Kills.RegisterKill(Main.npc[i]);
				}
				Main.npc[i].StrikeNPCNoInteraction(9999, 10f, -num2);
				num--;
				flag = true;
				NPC.SpawnWOF(position);
			}
		}
		if (!flag)
		{
			return;
		}
		List<int> list = new List<int>();
		for (int j = 0; j < Main.maxNPCs; j++)
		{
			if (num <= 0)
			{
				break;
			}
			NPC nPC = Main.npc[j];
			if (nPC.active && nPC.isLikeATownNPC)
			{
				list.Add(j);
			}
		}
		while (num > 0 && list.Count > 0)
		{
			int index = Main.rand.Next(list.Count);
			int num3 = list[index];
			list.RemoveAt(index);
			int num4 = -Main.npc[num3].direction;
			if (Main.npc[num3].IsNPCValidForBestiaryKillCredit())
			{
				Main.BestiaryTracker.Kills.RegisterKill(Main.npc[num3]);
			}
			Main.npc[num3].StrikeNPCNoInteraction(9999, 10f, -num4);
			num--;
		}
	}

	private bool CanShimmerAtPosition()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (!inner.CanShimmer())
		{
			return false;
		}
		int num = (int)(Center.X / 16f);
		int num2 = (int)(position.Y / 16f - 1f);
		Tile tile = Main.tile[num, num2];
		if (WorldGen.InWorld(num, num2) && tile != null && tile.liquid != 0)
		{
			return tile.shimmer();
		}
		return false;
	}

	private void MoveInWorld(float gravity, float maxFallSpeed, ref Vector2 wetVelocity)
	{
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		if (!shimmered && ItemID.Sets.ItemNoGravity[type])
		{
			velocity.X *= 0.95f;
			if ((double)velocity.X < 0.1 && (double)velocity.X > -0.1)
			{
				velocity.X = 0f;
			}
			velocity.Y *= 0.95f;
			if ((double)velocity.Y < 0.1 && (double)velocity.Y > -0.1)
			{
				velocity.Y = 0f;
			}
		}
		else
		{
			bool flag = false;
			if (shimmered && active)
			{
				int num = 50;
				for (int i = 0; i < 400; i++)
				{
					if (whoAmI == i || !Main.item[i].active || !Main.item[i].shimmered)
					{
						continue;
					}
					if (num-- <= 0)
					{
						break;
					}
					float num2 = (width + Main.item[i].width) / 2;
					if (!(Math.Abs(Center.X - Main.item[i].Center.X) <= num2) || !(Math.Abs(Center.Y - Main.item[i].Center.Y) <= num2))
					{
						continue;
					}
					flag = true;
					float num3 = Vector2.Distance(Center, Main.item[i].Center);
					num2 /= num3;
					if (num2 > 10f)
					{
						num2 = 10f;
					}
					if (Center.X < Main.item[i].Center.X)
					{
						if (velocity.X > -3f * num2)
						{
							velocity.X -= 0.1f * num2;
						}
						if (Main.item[i].velocity.X < 3f)
						{
							Main.item[i].velocity.X += 0.1f * num2;
						}
					}
					else if (Center.X > Main.item[i].Center.X)
					{
						if (velocity.X < 3f * num2)
						{
							velocity.X += 0.1f * num2;
						}
						if (Main.item[i].velocity.X > -3f)
						{
							Main.item[i].velocity.X -= 0.1f * num2;
						}
					}
					else if (whoAmI < i)
					{
						if (velocity.X > -3f * num2)
						{
							velocity.X -= 0.1f * num2;
						}
						if (Main.item[i].velocity.X < 3f * num2)
						{
							Main.item[i].velocity.X += 0.1f * num2;
						}
					}
				}
			}
			velocity.Y += gravity;
			if (velocity.Y > maxFallSpeed)
			{
				velocity.Y = maxFallSpeed;
			}
			velocity.X *= 0.95f;
			if ((double)velocity.X < 0.1 && (double)velocity.X > -0.1)
			{
				velocity.X = 0f;
			}
			if (flag)
			{
				velocity.X *= 0.8f;
			}
		}
		onConveyor = Collision.ApplyConveyorBeltMovementToVelocity(this, ref velocity);
		bool flag2 = Collision.LavaCollision(position, width, height);
		if (flag2)
		{
			lavaWet = true;
		}
		bool flag3 = Collision.WetCollision(position, width, height);
		if (Collision.honey)
		{
			honeyWet = true;
		}
		if (Collision.shimmer)
		{
			shimmerWet = true;
		}
		if (flag3)
		{
			if (!wet)
			{
				if (wetCount == 0)
				{
					wetCount = 20;
					if (!flag2)
					{
						if (shimmerWet)
						{
							for (int j = 0; j < 10; j++)
							{
								int num4 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 308);
								Main.dust[num4].velocity.Y -= 4f;
								Main.dust[num4].velocity.X *= 2.5f;
								Main.dust[num4].scale = 0.8f;
								Main.dust[num4].noGravity = true;
								switch (Main.rand.Next(6))
								{
								case 0:
									Main.dust[num4].color = new Color(255, 255, 210);
									break;
								case 1:
									Main.dust[num4].color = new Color(190, 245, 255);
									break;
								case 2:
									Main.dust[num4].color = new Color(255, 150, 255);
									break;
								default:
									Main.dust[num4].color = new Color(190, 175, 255);
									break;
								}
							}
							SoundEngine.PlaySound(19, (int)position.X, (int)position.Y, 4);
						}
						else if (honeyWet)
						{
							for (int k = 0; k < 5; k++)
							{
								int num5 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 152);
								Main.dust[num5].velocity.Y--;
								Main.dust[num5].velocity.X *= 2.5f;
								Main.dust[num5].scale = 1.3f;
								Main.dust[num5].alpha = 100;
								Main.dust[num5].noGravity = true;
							}
							SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
						}
						else
						{
							for (int l = 0; l < 10; l++)
							{
								int num6 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, Dust.dustWater());
								Main.dust[num6].velocity.Y -= 4f;
								Main.dust[num6].velocity.X *= 2.5f;
								Main.dust[num6].scale *= 0.8f;
								Main.dust[num6].alpha = 100;
								Main.dust[num6].noGravity = true;
							}
							SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
						}
					}
					else
					{
						for (int m = 0; m < 5; m++)
						{
							int num7 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 35);
							Main.dust[num7].velocity.Y -= 1.5f;
							Main.dust[num7].velocity.X *= 2.5f;
							Main.dust[num7].scale = 1.3f;
							Main.dust[num7].alpha = 100;
							Main.dust[num7].noGravity = true;
						}
						SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
					}
				}
				wet = true;
			}
		}
		else if (wet)
		{
			wet = false;
			if (wetCount == 0)
			{
				wetCount = 20;
				if (!lavaWet)
				{
					if (shimmerWet)
					{
						for (int n = 0; n < 10; n++)
						{
							int num8 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 308);
							Main.dust[num8].velocity.Y -= 4f;
							Main.dust[num8].velocity.X *= 2.5f;
							Main.dust[num8].scale = 0.8f;
							Main.dust[num8].noGravity = true;
							switch (Main.rand.Next(6))
							{
							case 0:
								Main.dust[num8].color = new Color(255, 255, 210);
								break;
							case 1:
								Main.dust[num8].color = new Color(190, 245, 255);
								break;
							case 2:
								Main.dust[num8].color = new Color(255, 150, 255);
								break;
							default:
								Main.dust[num8].color = new Color(190, 175, 255);
								break;
							}
						}
						SoundEngine.PlaySound(19, (int)position.X, (int)position.Y, 5);
					}
					else if (honeyWet)
					{
						for (int num9 = 0; num9 < 5; num9++)
						{
							int num10 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 152);
							Main.dust[num10].velocity.Y--;
							Main.dust[num10].velocity.X *= 2.5f;
							Main.dust[num10].scale = 1.3f;
							Main.dust[num10].alpha = 100;
							Main.dust[num10].noGravity = true;
						}
						SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
					}
					else
					{
						for (int num11 = 0; num11 < 10; num11++)
						{
							int num12 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2)), width + 12, 24, Dust.dustWater());
							Main.dust[num12].velocity.Y -= 4f;
							Main.dust[num12].velocity.X *= 2.5f;
							Main.dust[num12].scale *= 0.8f;
							Main.dust[num12].alpha = 100;
							Main.dust[num12].noGravity = true;
						}
						SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
					}
				}
				else
				{
					for (int num13 = 0; num13 < 5; num13++)
					{
						int num14 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 35);
						Main.dust[num14].velocity.Y -= 1.5f;
						Main.dust[num14].velocity.X *= 2.5f;
						Main.dust[num14].scale = 1.3f;
						Main.dust[num14].alpha = 100;
						Main.dust[num14].noGravity = true;
					}
					SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
				}
			}
		}
		if (!wet)
		{
			lavaWet = false;
			honeyWet = false;
			shimmerWet = false;
		}
		if (wetCount > 0)
		{
			wetCount--;
		}
		if (wet)
		{
			if (wet)
			{
				Vector2 val = velocity;
				velocity = Collision.TileCollision(position, velocity, width, height, fallThrough: false, fall2: false, 1, ignoreDoors: false, ignoreAetheriumPlatforms: true);
				if (velocity.X != val.X)
				{
					wetVelocity.X = velocity.X;
				}
				if (velocity.Y != val.Y)
				{
					wetVelocity.Y = velocity.Y;
				}
			}
		}
		else
		{
			velocity = Collision.TileCollision(position, velocity, width, height, fallThrough: false, fall2: false, 1, ignoreDoors: false, ignoreAetheriumPlatforms: true);
		}
		ApplyMovement(ref wetVelocity);
		Vector4 val2 = Collision.SlopeCollision(position, velocity, width, height, gravity, fall: false, ignoreAetheriumPlatforms: true);
		position.X = val2.X;
		position.Y = val2.Y;
		velocity.X = val2.Z;
		velocity.Y = val2.W;
	}

	private void ApplyMovement(ref Vector2 wetVelocity)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (wet)
		{
			position += wetVelocity;
		}
		else
		{
			position += velocity;
		}
	}

	private void GetPickedUpByMonsters_Special()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = false;
		int num = type;
		if ((num == 89 || num == 3507) && !NPC.unlockedSlimeCopperSpawn)
		{
			flag = true;
			flag2 = true;
		}
		if (!flag2)
		{
			return;
		}
		bool flag3 = false;
		Rectangle hitbox = Hitbox;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active && flag && nPC.type >= 0 && nPC.type < NPCID.Count && NPCID.Sets.CanConvertIntoCopperSlimeTownNPC[nPC.type] && hitbox.Intersects(nPC.Hitbox))
			{
				flag3 = true;
				NPC.TransformCopperSlime(i);
				break;
			}
		}
		if (flag3)
		{
			TurnToAirAndSync();
		}
	}

	private void GetPickedUpByMonsters_Money()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = new Rectangle((int)position.X, (int)position.Y, width, height);
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (!nPC.active || nPC.lifeMax <= 5 || nPC.friendly || nPC.immortal || nPC.dontTakeDamage || nPC.SpawnedFromStatue || NPCID.Sets.CantTakeLunchMoney[nPC.type])
			{
				continue;
			}
			float num = stack;
			float num2 = 1f;
			if (type == 72)
			{
				num2 = 100f;
			}
			if (type == 73)
			{
				num2 = 10000f;
			}
			if (type == 74)
			{
				num2 = 1000000f;
			}
			num *= num2;
			float num3 = nPC.extraValue;
			int num4 = nPC.realLife;
			NPC nPC2 = nPC;
			if (num4 >= 0 && Main.npc[num4].active)
			{
				nPC2 = Main.npc[num4];
				num3 = nPC2.extraValue;
			}
			else
			{
				num4 = -1;
			}
			if (!(num3 < num) || !(num3 + num < 999000000f))
			{
				continue;
			}
			Rectangle val2 = new Rectangle((int)nPC.position.X, (int)nPC.position.Y, nPC.width, nPC.height);
			if (val.Intersects(val2))
			{
				float num5 = (float)Main.rand.Next(50, 76) * 0.01f;
				if (type == 71)
				{
					num5 += (float)Main.rand.Next(51) * 0.01f;
				}
				if (type == 72)
				{
					num5 += (float)Main.rand.Next(26) * 0.01f;
				}
				if (num5 > 1f)
				{
					num5 = 1f;
				}
				int num6 = (int)((float)stack * num5);
				if (num6 < 1)
				{
					num6 = 1;
				}
				if (num6 > stack)
				{
					num6 = stack;
				}
				stack -= num6;
				int num7 = (int)((float)num6 * num2);
				int number = i;
				if (num4 >= 0)
				{
					number = num4;
				}
				nPC2.extraValue += num7;
				if (Main.netMode == 0)
				{
					nPC2.moneyPing(position);
				}
				else
				{
					NetMessage.SendData(92, -1, -1, null, number, num7, position.X, position.Y);
				}
				if (stack <= 0)
				{
					TurnToAir();
				}
				SyncItem();
			}
		}
	}

	private void UpdateItem_VisualEffects()
	{
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f20: Unknown result type (might be due to invalid IL or missing references)
		//IL_184c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1937: Unknown result type (might be due to invalid IL or missing references)
		//IL_1946: Unknown result type (might be due to invalid IL or missing references)
		//IL_1977: Unknown result type (might be due to invalid IL or missing references)
		//IL_1986: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2006: Unknown result type (might be due to invalid IL or missing references)
		//IL_2021: Unknown result type (might be due to invalid IL or missing references)
		//IL_2026: Unknown result type (might be due to invalid IL or missing references)
		//IL_204c: Unknown result type (might be due to invalid IL or missing references)
		//IL_205a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2060: Unknown result type (might be due to invalid IL or missing references)
		if (type == 5043)
		{
			float num = (float)Main.rand.Next(90, 111) * 0.01f;
			num *= (Main.essScale + 0.5f) / 2f;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f * num, 0.25f * num, 0.25f * num);
		}
		else if (type == 116)
		{
			float num2 = (float)Main.rand.Next(95, 106) * 0.01f;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.56f * num2, 0.43f * num2, 0.15f * num2);
			if (Main.rand.Next(250) == 0)
			{
				int num3 = Dust.NewDust(position, width, height, 6, 0f, 0f, 0, default, Main.rand.Next(3));
				if (Main.dust[num3].scale > 1f)
				{
					Main.dust[num3].noGravity = true;
				}
			}
		}
		else if (type == 3191)
		{
			float num4 = (float)Main.rand.Next(90, 111) * 0.01f;
			num4 *= (Main.essScale + 0.5f) / 2f;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.3f * num4, 0.1f * num4, 0.25f * num4);
		}
		else if (type == 520 || type == 3454)
		{
			float num5 = (float)Main.rand.Next(90, 111) * 0.01f;
			num5 *= Main.essScale;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num5, 0.1f * num5, 0.25f * num5);
		}
		else if (type == 521 || type == 3455)
		{
			float num6 = (float)Main.rand.Next(90, 111) * 0.01f;
			num6 *= Main.essScale;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f * num6, 0.1f * num6, 0.5f * num6);
		}
		else if (type == 547 || type == 3453)
		{
			float num7 = (float)Main.rand.Next(90, 111) * 0.01f;
			num7 *= Main.essScale;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num7, 0.3f * num7, 0.05f * num7);
		}
		else if (type == 548)
		{
			float num8 = (float)Main.rand.Next(90, 111) * 0.01f;
			num8 *= Main.essScale;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num8, 0.1f * num8, 0.6f * num8);
		}
		else if (type == 575)
		{
			float num9 = (float)Main.rand.Next(90, 111) * 0.01f;
			num9 *= Main.essScale;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num9, 0.3f * num9, 0.5f * num9);
		}
		else if (type == 549)
		{
			float num10 = (float)Main.rand.Next(90, 111) * 0.01f;
			num10 *= Main.essScale;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num10, 0.5f * num10, 0.2f * num10);
		}
		else if (type == 58 || type == 1734 || type == 1867)
		{
			float num11 = (float)Main.rand.Next(90, 111) * 0.01f;
			num11 *= Main.essScale * 0.5f;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num11, 0.1f * num11, 0.1f * num11);
		}
		else if (type == 184 || type == 1735 || type == 1868 || type == 4143)
		{
			float num12 = (float)Main.rand.Next(90, 111) * 0.01f;
			num12 *= Main.essScale * 0.5f;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num12, 0.1f * num12, 0.5f * num12);
		}
		else if (type == 522)
		{
			float num13 = (float)Main.rand.Next(90, 111) * 0.01f;
			num13 *= Main.essScale * 0.2f;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num13, 1f * num13, 0.1f * num13);
		}
		else if (type == 1332)
		{
			float num14 = (float)Main.rand.Next(90, 111) * 0.01f;
			num14 *= Main.essScale * 0.2f;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f * num14, 1f * num14, 0.1f * num14);
		}
		else if (type == 3456)
		{
			Lighting.AddLight(Center, new Vector3(0.2f, 0.4f, 0.5f) * Main.essScale);
		}
		else if (type == 3457)
		{
			Lighting.AddLight(Center, new Vector3(0.4f, 0.2f, 0.5f) * Main.essScale);
		}
		else if (type == 3458)
		{
			Lighting.AddLight(Center, new Vector3(0.5f, 0.4f, 0.2f) * Main.essScale);
		}
		else if (type == 3459)
		{
			Lighting.AddLight(Center, new Vector3(0.2f, 0.2f, 0.5f) * Main.essScale);
		}
		else if (type == 501)
		{
			if (Main.rand.Next(6) == 0)
			{
				int num15 = Dust.NewDust(position, width, height, 55, 0f, 0f, 200, color);
				Dust obj = Main.dust[num15];
				obj.velocity *= 0.3f;
				Main.dust[num15].scale *= 0.5f;
			}
		}
		else if (type == 3822)
		{
			Lighting.AddLight(Center, 0.1f, 0.3f, 0.1f);
		}
		else if (type == 1970)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0f, 0.75f);
		}
		else if (type == 1972)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0f, 0.75f);
		}
		else if (type == 1971)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.75f, 0f);
		}
		else if (type == 1973)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0.75f, 0f);
		}
		else if (type == 1974)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0f, 0f);
		}
		else if (type == 1975)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.75f, 0.75f);
		}
		else if (type == 1976)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.375f, 0f);
		}
		else if (type == 2679)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0f, 0.6f);
		}
		else if (type == 2687)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0f, 0.6f);
		}
		else if (type == 2689)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0.6f, 0f);
		}
		else if (type == 2683)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0.6f, 0f);
		}
		else if (type == 2685)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0f, 0f);
		}
		else if (type == 2681)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0.6f, 0.6f);
		}
		else if (type == 2677)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0.375f, 0f);
		}
		else if (type == 105)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.95f, 0.8f);
			}
		}
		else if (type == 2701)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 0.65f, 0.55f);
		}
		else if (inner.createTile == 4)
		{
			int placeStyle = inner.placeStyle;
			if ((!wet && ItemID.Sets.Torches[type]) || ItemID.Sets.WaterTorches[type])
			{
				Lighting.AddLight(Center, placeStyle);
			}
		}
		else if (type == 3114)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0f, 1f);
			}
		}
		else if (type == 1245)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.5f, 0f);
			}
		}
		else if (type == 433)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch), 0.3f, 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch));
			}
		}
		else if (type == 523)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.85f, 1.2f, 0.7f);
		}
		else if (type == 974)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.85f, 1.4f);
			}
		}
		else if (type == 1333)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1.25f, 1.25f, 0.7f);
		}
		else if (type == 4383)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1.4f, 0.85f, 0.55f);
			}
		}
		else if (type == 5293)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f, 0.65f, 1f);
			}
		}
		else if (type == 5353)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.81f, 0.72f, 1f);
			}
		}
		else if (type == 4384)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f, 1.3f, 0.8f);
		}
		else if (type == 3045)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), (float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f);
		}
		else if (type == 3004)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.95f, 0.65f, 1.3f);
		}
		else if (type == 2274)
		{
			float r = 0.75f;
			float g = 1.3499999f;
			float b = 1.5f;
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), r, g, b);
			}
		}
		else if (type >= 427 && type <= 432)
		{
			if (!wet)
			{
				float r2 = 0f;
				float g2 = 0f;
				float b2 = 0f;
				int num16 = type - 426;
				if (num16 == 1)
				{
					r2 = 0.1f;
					g2 = 0.2f;
					b2 = 1.1f;
				}
				if (num16 == 2)
				{
					r2 = 1f;
					g2 = 0.1f;
					b2 = 0.1f;
				}
				if (num16 == 3)
				{
					r2 = 0f;
					g2 = 1f;
					b2 = 0.1f;
				}
				if (num16 == 4)
				{
					r2 = 0.9f;
					g2 = 0f;
					b2 = 0.9f;
				}
				if (num16 == 5)
				{
					r2 = 1.3f;
					g2 = 1.3f;
					b2 = 1.3f;
				}
				if (num16 == 6)
				{
					r2 = 0.9f;
					g2 = 0.9f;
					b2 = 0f;
				}
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), r2, g2, b2);
			}
		}
		else if (type == 2777 || type == 2778 || type == 2779 || type == 2780 || type == 2781 || type == 2760 || type == 2761 || type == 2762 || type == 3524)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.4f, 0.16f, 0.36f);
		}
		else if (type == 2772 || type == 2773 || type == 2774 || type == 2775 || type == 2776 || type == 2757 || type == 2758 || type == 2759 || type == 3523)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0.36f, 0.4f);
		}
		else if (type == 2782 || type == 2783 || type == 2784 || type == 2785 || type == 2786 || type == 2763 || type == 2764 || type == 2765 || type == 3522)
		{
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f, 0.25f, 0.05f);
		}
		else if (type == 3462 || type == 3463 || type == 3464 || type == 3465 || type == 3466 || type == 3381 || type == 3382 || type == 3383 || type == 3525)
		{
			Lighting.AddLight(Center, 0.3f, 0.3f, 0.2f);
		}
		else if (type == 41)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.75f, 0.55f);
			}
		}
		else if (type == 988)
		{
			if (!wet)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.35f, 0.65f, 1f);
			}
		}
		else if (type == 1326)
		{
			Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 1f, 0.1f, 0.8f);
		}
		else if (type == 5335)
		{
			Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.85f, 0.1f, 0.8f);
		}
		else if (type >= 5140 && type <= 5146)
		{
			float num17 = 1f;
			float num18 = 1f;
			float num19 = 1f;
			switch (type)
			{
			case 5140:
				num17 *= 0.9f;
				num18 *= 0.8f;
				num19 *= 0.1f;
				break;
			case 5141:
				num17 *= 0.25f;
				num18 *= 0.1f;
				num19 *= 0f;
				break;
			case 5142:
				num17 *= 0f;
				num18 *= 0.25f;
				num19 *= 0f;
				break;
			case 5143:
				num17 *= 0f;
				num18 *= 0.16f;
				num19 *= 0.34f;
				break;
			case 5144:
				num17 *= 0.3f;
				num18 *= 0f;
				num19 *= 0.17f;
				break;
			case 5145:
				num17 *= 0.3f;
				num18 *= 0f;
				num19 *= 0.35f;
				break;
			case 5146:
				num17 *= (float)Main.DiscoR / 255f;
				num18 *= (float)Main.DiscoG / 255f;
				num19 *= (float)Main.DiscoB / 255f;
				break;
			}
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), num17, num18, num19);
		}
		else if (type == 282)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 1f, 0.8f);
		}
		else if (type == 286)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 0.8f, 1f);
		}
		else if (type == 3112)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.6f, 0.85f);
		}
		else if (type == 4776)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 0f, 1f);
		}
		else if (type == 3002)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1.05f, 0.95f, 0.55f);
		}
		else if (type == 5643)
		{
			float r3 = (float)Main.DiscoR / 255f;
			float g3 = (float)Main.DiscoG / 255f;
			float b3 = (float)Main.DiscoB / 255f;
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), r3, g3, b3);
		}
		else if (type == 331)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.55f, 0.75f, 0.6f);
		}
		else if (type == 183)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.15f, 0.45f, 0.9f);
		}
		else if (type == 75)
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.8f, 0.7f, 0.1f);
			if (timeSinceItemSpawned % 12 == 0)
			{
				Dust dust = Dust.NewDustPerfect(Center + new Vector2(0f, (float)height * 0.2f) + Main.rand.NextVector2CircularEdge(width, (float)height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 228, new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f), 127);
				dust.scale = 0.5f;
				dust.fadeIn = 1.1f;
				dust.noGravity = true;
				dust.noLight = true;
			}
		}
		else if (ItemID.Sets.BossBag[type])
		{
			Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.4f, 0.4f, 0.4f);
			if (timeSinceItemSpawned % 12 == 0)
			{
				Dust dust2 = Dust.NewDustPerfect(Center + new Vector2(0f, (float)height * -0.1f) + Main.rand.NextVector2CircularEdge((float)width * 0.6f, (float)height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 279, new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f), 127);
				dust2.scale = 0.5f;
				dust2.fadeIn = 1.1f;
				dust2.noGravity = true;
				dust2.noLight = true;
				dust2.alpha = 0;
			}
		}
	}

	public IEntitySource GetNPCSource_FromThis()
	{
		return new EntitySource_Parent(this);
	}

	public IEntitySource GetItemSource_Misc(int itemSourceId)
	{
		return new EntitySource_ByItemSourceId(this, itemSourceId);
	}

	public static void ShimmerEffect(Vector2 shimmerPositon)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2)
		{
			NetMessage.SendData(146, -1, -1, null, 0, shimmerPositon.X, shimmerPositon.Y);
			return;
		}
		SoundEngine.PlaySound(SoundID.Item176, (int)shimmerPositon.X, (int)shimmerPositon.Y);
		for (int i = 0; i < 20; i++)
		{
			int num = Dust.NewDust(shimmerPositon, 1, 1, 309);
			Main.dust[num].scale *= 1.2f;
			switch (Main.rand.Next(6))
			{
			case 0:
				Main.dust[num].color = new Color(255, 255, 210);
				break;
			case 1:
				Main.dust[num].color = new Color(190, 245, 255);
				break;
			case 2:
				Main.dust[num].color = new Color(255, 150, 255);
				break;
			default:
				Main.dust[num].color = new Color(190, 175, 255);
				break;
			}
		}
	}

	public void GetShimmered()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		int shimmerEquivalentType = inner.GetShimmerEquivalentType();
		int decraftingRecipeIndex = ShimmerTransforms.GetDecraftingRecipeIndex(inner.GetShimmerEquivalentType(forDecrafting: true));
		int transformToItem = ShimmerTransforms.GetTransformToItem(shimmerEquivalentType);
		int makeNPC = inner.makeNPC;
		if (ItemID.Sets.CommonCoin[shimmerEquivalentType])
		{
			switch (shimmerEquivalentType)
			{
			case 72:
				stack *= 100;
				break;
			case 73:
				stack *= 10000;
				break;
			case 74:
				if (stack > 1)
				{
					stack = 1;
				}
				stack *= 1000000;
				break;
			}
			Main.player[Main.myPlayer].AddCoinLuck(Center, stack);
			NetMessage.SendData(146, -1, -1, null, 1, Center.X, Center.Y, stack);
			type = 0;
			stack = 0;
		}
		else if (transformToItem > 0)
		{
			int num = stack;
			inner.SetDefaults(transformToItem);
			stack = num;
			shimmered = true;
		}
		else if (type == 4986)
		{
			if (NPC.unlockedSlimeRainbowSpawn)
			{
				return;
			}
			NPC.unlockedSlimeRainbowSpawn = true;
			NetMessage.SendData(7);
			int num2 = NPC.NewNPC(GetNPCSource_FromThis(), (int)Center.X + 4, (int)Center.Y, 681);
			if (num2 >= 0)
			{
				NPC obj = Main.npc[num2];
				obj.velocity = velocity;
				obj.shimmerTransparency = 1f;
			}
			WorldGen.CheckAchievement_RealEstateAndTownSlimes();
			stack--;
			if (stack <= 0)
			{
				type = 0;
			}
		}
		else if (type == 560)
		{
			if (Main.slimeRain)
			{
				return;
			}
			Main.StartSlimeRain();
			stack--;
			if (stack <= 0)
			{
				type = 0;
			}
			else
			{
				shimmered = true;
			}
		}
		else if (makeNPC > 0)
		{
			int num3 = 50;
			int maxNPCs = Main.maxNPCs;
			int num4 = NPC.GetAvailableAmountOfNPCsToSpawnUpToSlot(stack, maxNPCs);
			while (num3 > 0 && num4 > 0 && stack > 0)
			{
				num3--;
				num4--;
				stack--;
				int num5 = -1;
				num5 = ((NPCID.Sets.ShimmerTransformToNPC[makeNPC] < 0) ? NPC.ReleaseNPC((int)Center.X, (int)Bottom.Y, makeNPC, inner.placeStyle, Main.myPlayer) : NPC.ReleaseNPC((int)Center.X, (int)Bottom.Y, NPCID.Sets.ShimmerTransformToNPC[makeNPC], 0, Main.myPlayer));
				if (num5 >= 0)
				{
					Main.npc[num5].shimmerTransparency = 1f;
				}
			}
			shimmered = true;
			if (stack <= 0)
			{
				type = 0;
			}
		}
		else if (decraftingRecipeIndex >= 0)
		{
			int num6 = inner.FindDecraftAmount();
			Recipe recipe = Main.recipe[decraftingRecipeIndex];
			_ = recipe.requiredItem[1].stack;
			IEnumerable<Recipe.RequiredItemEntry> enumerable = recipe.requiredItemQuickLookup;
			if (recipe.customShimmerResults != null)
			{
				enumerable = recipe.customShimmerResults.Select((Item item) => new Recipe.RequiredItemEntry
				{
					itemIdOrRecipeGroup = item.type,
					stack = item.stack
				});
			}
			int num7 = 0;
			foreach (Recipe.RequiredItemEntry item in enumerable)
			{
				if (item.itemIdOrRecipeGroup <= 0)
				{
					break;
				}
				int num8 = num6 * item.stack;
				int key = (item.IsRecipeGroup ? item.RecipeGroup.DecraftItemId : item.itemIdOrRecipeGroup);
				if (recipe.alchemy)
				{
					for (int num9 = num8; num9 > 0; num9--)
					{
						if (Main.rand.Next(3) == 0)
						{
							num8--;
						}
					}
				}
				while (num8 > 0)
				{
					int num10 = Math.Min(num8, ContentSamples.ItemsByType[key].maxStack);
					num8 -= num10;
					SpawnShimmeredItem(num7++, key, num10);
				}
			}
			stack -= num6 * recipe.createItem.stack;
			if (stack <= 0)
			{
				stack = 0;
				type = 0;
			}
		}
		if (stack > 0)
		{
			shimmerTime = 1f;
		}
		else
		{
			shimmerTime = 0f;
		}
		shimmerWet = true;
		wet = true;
		velocity *= 0.1f;
		ShimmerEffect(Center);
		SyncItem();
		AchievementsHelper.NotifyProgressionEvent(27);
		if (stack == 0)
		{
			TurnToAir();
		}
	}

	private static void SyncItemsInSection(int toClient, Point sectionCoordinates)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = new Rectangle(sectionCoordinates.X * 200 * 16, sectionCoordinates.Y * 150 * 16, 3200, 2400);
		val.Inflate(16, 16);
		for (int i = 0; i < 400; i++)
		{
			WorldItem worldItem = Main.item[i];
			if (worldItem.active && val.Contains(worldItem.Center.ToPoint()))
			{
				NetMessage.SendData(160, toClient, -1, null, i);
			}
		}
	}
}
