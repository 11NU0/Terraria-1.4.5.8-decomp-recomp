using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.Testing;

namespace Terraria;

public class Chest : IFixLoadedData
{
	public struct ItemTransferVisualizationSettings
	{
		public bool RandomizeStartPosition;

		public bool RandomizeEndPosition;

		public bool TransitionIn;

		public bool Fullbright;

		public static ItemTransferVisualizationSettings PlayerToChest = new ItemTransferVisualizationSettings
		{
			RandomizeStartPosition = true,
			RandomizeEndPosition = true,
			TransitionIn = true,
			Fullbright = true
		};

		public static ItemTransferVisualizationSettings Hopper = new ItemTransferVisualizationSettings
		{
			RandomizeEndPosition = true
		};
	}

	public const float chestStackRange = 600f;

	public const int maxChestTypes = 52;

	public static int[] chestTypeToIcon = new int[52];

	public const int maxChestTypes2 = 38;

	public static int[] chestTypeToIcon2 = new int[38];

	public const int maxDresserTypes = 65;

	public static int[] dresserTypeToIcon = new int[65];

	public const int DefaultMaxItems = 40;

	public const int AbsoluteMaxItemsWeCanEverReachInAChestForNow = 200;

	public int maxItems;

	public const int MaxNameLength = 20;

	public Item[] item;

	public readonly int x;

	public readonly int y;

	public readonly int index;

	public bool bankChest;

	public string name;

	public int frameCounter;

	public int frame;

	public int eatingAnimationTime;

	private bool _itemsGotSet;

	private static Dictionary<Point, Chest> _chestsByCoords = new Dictionary<Point, Chest>();

	private static ParticlePool<RoomCheckParticle> _particlePool = new ParticlePool<RoomCheckParticle>(100, GetNewParticle);

	private static HashSet<int> _chestInUse = new HashSet<int>();

	public static void Clear()
	{
		Array.Clear(Main.chest, 0, Main.chest.Length);
		_chestsByCoords.Clear();
	}

	public static Chest CreateWorldChest(int index, int x, int y)
	{
		Chest chest = new Chest(index, x, y);
		chest.FillWithEmptyInstances();
		Assign(chest);
		return chest;
	}

	public static void Assign(Chest chest)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Main.chest[chest.index] = chest;
		_chestsByCoords[new Point(chest.x, chest.y)] = chest;
	}

	public void Resize(int newSize)
	{
		int num = maxItems;
		maxItems = newSize;
		Array.Resize(ref item, newSize);
		if (_itemsGotSet)
		{
			for (int i = num; i < newSize; i++)
			{
				item[i] = new Item();
			}
		}
	}

	public static void RemoveChest(int chestIndex)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Chest chest = Main.chest[chestIndex];
		if (chest != null)
		{
			_chestsByCoords.Remove(new Point(chest.x, chest.y));
		}
		Main.chest[chestIndex] = null;
	}

	public static Chest CreateBank(int index)
	{
		return new Chest(index, 0, 0, bank: true);
	}

	public static Chest CreateShop()
	{
		return new Chest();
	}

	public static Chest CreateOutOfArray(int index, int x, int y, int maxItems)
	{
		return new Chest(index, x, y, bank: false, maxItems);
	}

	public void FillWithEmptyInstances()
	{
		for (int i = 0; i < maxItems; i++)
		{
			item[i] = new Item();
		}
		_itemsGotSet = true;
	}

	public bool IsEmpty()
	{
		for (int i = 0; i < maxItems; i++)
		{
			if (!item[i].IsAir)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsLockedOrInUse()
	{
		if (!bankChest)
		{
			if (!IsPlayerInChest(index))
			{
				return IsLocked(x, y);
			}
			return true;
		}
		return false;
	}

	private Chest(int index = 0, int x = 0, int y = 0, bool bank = false, int maxItems = 40)
	{
		this.maxItems = maxItems;
		item = new Item[maxItems];
		this.index = index;
		this.x = x;
		this.y = y;
		bankChest = bank;
		name = string.Empty;
	}

	public override string ToString()
	{
		int num = 0;
		for (int i = 0; i < item.Length; i++)
		{
			if (item[i].stack > 0)
			{
				num++;
			}
		}
		return $"{{X: {x}, Y: {y}, Count: {num}}}";
	}

	public Chest CloneWithSeparateItems()
	{
		Chest chest = new Chest(index, x, y, bankChest, maxItems)
		{
			name = name
		};
		for (int i = 0; i < maxItems; i++)
		{
			chest.item[i] = item[i].Clone();
			chest._itemsGotSet = true;
		}
		return chest;
	}

	public static void Initialize()
	{
		int[] array = chestTypeToIcon;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = WorldGen.GetItemDrop_Chests(i, secondType: false);
		}
		array[2] = 327;
		array[4] = 329;
		array[23] = 1533;
		array[24] = 1534;
		array[25] = 1535;
		array[26] = 1536;
		array[27] = 1537;
		array[36] = 327;
		array[38] = 327;
		array[40] = 327;
		int[] array2 = chestTypeToIcon2;
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = WorldGen.GetItemDrop_Chests(j, secondType: true);
		}
		array2[13] = 4714;
		int[] array3 = dresserTypeToIcon;
		for (int k = 0; k < dresserTypeToIcon.Length; k++)
		{
			array3[k] = WorldGen.GetItemDrop_Dressers(k);
		}
	}

	internal static void AddGameplaySnapshotComponents()
	{
		StateSnapshot.Gameplay.AddRef("Chest._chestsByCoords", () => _chestsByCoords, (Dictionary<Point, Chest> v) =>
		{
			_chestsByCoords = v;
		});
		StateSnapshot.Gameplay.AddRef("Chest._chestInUse", () => _chestInUse, (HashSet<int> v) =>
		{
			_chestInUse = v;
		});
	}

	public static bool IsPlayerInChest(int i)
	{
		for (int j = 0; j < 255; j++)
		{
			if (Main.player[j].chest == i)
			{
				return true;
			}
		}
		return false;
	}

	public static List<int> GetCurrentlyOpenChests()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].chest > -1)
			{
				list.Add(Main.player[i].chest);
			}
		}
		return list;
	}

	public static bool IsLocked(int x, int y)
	{
		return IsLocked(Main.tile[x, y]);
	}

	public static bool IsLocked(Tile t)
	{
		if (t == null)
		{
			return true;
		}
		if (t.type == 21 && ((t.frameX >= 72 && t.frameX <= 106) || (t.frameX >= 144 && t.frameX <= 178) || (t.frameX >= 828 && t.frameX <= 1006) || (t.frameX >= 1296 && t.frameX <= 1330) || (t.frameX >= 1368 && t.frameX <= 1402) || (t.frameX >= 1440 && t.frameX <= 1474)))
		{
			return true;
		}
		if (t.type == 467)
		{
			return t.frameX / 36 == 13;
		}
		return false;
	}

	public static void VisualizeChestTransfer(Vector2 position, Vector2 chestPosition, int itemType, ItemTransferVisualizationSettings settings)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		BitsByte bitsByte = new BitsByte(settings.RandomizeStartPosition, settings.RandomizeEndPosition, settings.TransitionIn, settings.Fullbright);
		ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ItemTransfer, new ParticleOrchestraSettings
		{
			PositionInWorld = position,
			MovementVector = chestPosition - position,
			UniqueInfoPiece = (itemType | ((byte)bitsByte << 24))
		});
	}

	public static void VisualizeChestTransfer_CoinsBatch(Vector2 position, Vector2 chestPosition, long coinsMoved, ItemTransferVisualizationSettings settings)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		int[] array = Utils.CoinsSplit(coinsMoved);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] >= 1)
			{
				VisualizeChestTransfer(position, chestPosition, 71 + i, settings);
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public static bool Unlock(int X, int Y)
	{
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		if (Main.tile[X, Y] == null || Main.tile[X + 1, Y] == null || Main.tile[X, Y + 1] == null || Main.tile[X + 1, Y + 1] == null)
		{
			return false;
		}
		short num = 0;
		int type = 0;
		Tile tileSafely = Framing.GetTileSafely(X, Y);
		int type2 = tileSafely.type;
		int num2 = tileSafely.frameX / 36;
		switch (type2)
		{
		case 21:
			switch (num2)
			{
			case 2:
				num = 36;
				type = 11;
				AchievementsHelper.NotifyProgressionEvent(19);
				break;
			case 4:
				num = 36;
				type = 11;
				break;
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
				if (!NPC.downedPlantBoss)
				{
					return false;
				}
				num = 180;
				type = 11;
				AchievementsHelper.NotifyProgressionEvent(20);
				break;
			case 36:
			case 38:
			case 40:
				num = 36;
				type = 11;
				break;
			default:
				return false;
			}
			break;
		case 467:
			if (num2 == 13)
			{
				if (!NPC.downedPlantBoss)
				{
					return false;
				}
				num = 36;
				type = 11;
				AchievementsHelper.NotifyProgressionEvent(20);
				break;
			}
			return false;
		}
		SoundEngine.PlaySound(22, X * 16, Y * 16);
		for (int i = X; i <= X + 1; i++)
		{
			for (int j = Y; j <= Y + 1; j++)
			{
				Tile tileSafely2 = Framing.GetTileSafely(i, j);
				if (tileSafely2.type == type2)
				{
					tileSafely2.frameX -= num;
					Main.tile[i, j] = tileSafely2;
					for (int k = 0; k < 4; k++)
					{
						Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, type);
					}
				}
			}
		}
		return true;
	}

	public static bool Lock(int X, int Y)
	{
		if (Main.tile[X, Y] == null || Main.tile[X + 1, Y] == null || Main.tile[X, Y + 1] == null || Main.tile[X + 1, Y + 1] == null)
		{
			return false;
		}
		short num = 0;
		Tile tileSafely = Framing.GetTileSafely(X, Y);
		int type = tileSafely.type;
		int num2 = tileSafely.frameX / 36;
		switch (type)
		{
		case 21:
			switch (num2)
			{
			case 1:
				num = 36;
				break;
			case 3:
				num = 36;
				break;
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
				if (!NPC.downedPlantBoss)
				{
					return false;
				}
				num = 180;
				break;
			case 35:
			case 37:
			case 39:
				num = 36;
				break;
			default:
				return false;
			}
			break;
		case 467:
			if (num2 == 12)
			{
				if (!NPC.downedPlantBoss)
				{
					return false;
				}
				num = 36;
				AchievementsHelper.NotifyProgressionEvent(20);
				break;
			}
			return false;
		}
		SoundEngine.PlaySound(22, X * 16, Y * 16);
		for (int i = X; i <= X + 1; i++)
		{
			for (int j = Y; j <= Y + 1; j++)
			{
				Tile tileSafely2 = Framing.GetTileSafely(i, j);
				if (tileSafely2.type == type)
				{
					tileSafely2.frameX += num;
					Main.tile[i, j] = tileSafely2;
				}
			}
		}
		return true;
	}

	public static int UsingChest(int i)
	{
		if (Main.chest[i] != null)
		{
			for (int j = 0; j < 255; j++)
			{
				if (Main.player[j].active && Main.player[j].chest == i)
				{
					return j;
				}
			}
		}
		return -1;
	}

	public static int FindChest(int X, int Y)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (_chestsByCoords.TryGetValue(new Point(X, Y), out var value))
		{
			return value.index;
		}
		return -1;
	}

	public static int FindEmptyChest(int x, int y, int type = 21, int style = 0, int direction = 1, int alternate = 0)
	{
		int num = -1;
		for (int i = 0; i < 8000; i++)
		{
			Chest chest = Main.chest[i];
			if (chest != null)
			{
				if (chest.x == x && chest.y == y)
				{
					return -1;
				}
			}
			else if (num == -1)
			{
				num = i;
			}
		}
		return num;
	}

	public static bool NearOtherChests(int x, int y)
	{
		for (int i = x - 25; i < x + 25; i++)
		{
			for (int j = y - 8; j < y + 8; j++)
			{
				Tile tileSafely = Framing.GetTileSafely(i, j);
				if (tileSafely.active() && TileID.Sets.BasicChest[tileSafely.type])
				{
					return true;
				}
			}
		}
		return false;
	}

	public static int AfterPlacement_Hook(int x, int y, int type = 21, int style = 0, int direction = 1, int alternate = 0)
	{
		Point16 baseCoords = new Point16(x, y);
		TileObjectData.OriginToTopLeft(type, style, ref baseCoords);
		int num = FindEmptyChest(baseCoords.X, baseCoords.Y);
		if (num == -1)
		{
			return -1;
		}
		if (Main.netMode != 1)
		{
			CreateWorldChest(num, baseCoords.X, baseCoords.Y);
		}
		else
		{
			switch (type)
			{
			case 21:
				NetMessage.SendData(34, -1, -1, null, 0, x, y, style);
				break;
			case 467:
				NetMessage.SendData(34, -1, -1, null, 4, x, y, style);
				break;
			default:
				NetMessage.SendData(34, -1, -1, null, 2, x, y, style);
				break;
			}
		}
		return num;
	}

	public static int CreateChest(int X, int Y, int id = -1)
	{
		int num = id;
		if (num == -1)
		{
			num = FindEmptyChest(X, Y);
			if (num == -1)
			{
				return -1;
			}
			if (Main.netMode == 1)
			{
				return num;
			}
		}
		CreateWorldChest(num, X, Y);
		return num;
	}

	public static bool CanDestroyChest(int X, int Y)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (!_chestsByCoords.TryGetValue(new Point(X, Y), out var value))
		{
			return true;
		}
		for (int i = 0; i < value.maxItems; i++)
		{
			if (value.item[i] != null && value.item[i].type > 0 && value.item[i].stack > 0)
			{
				return false;
			}
		}
		return true;
	}

	public static bool DestroyChest(int X, int Y)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (!_chestsByCoords.TryGetValue(new Point(X, Y), out var value))
		{
			return true;
		}
		for (int i = 0; i < value.maxItems; i++)
		{
			if (value.item[i] != null && value.item[i].type > 0 && value.item[i].stack > 0)
			{
				return false;
			}
		}
		int num = value.index;
		RemoveChest(num);
		if (Main.player[Main.myPlayer].chest == num)
		{
			Main.player[Main.myPlayer].chest = -1;
		}
		return true;
	}

	public static void DestroyChestDirect(int X, int Y, int id)
	{
		if (id < 0 || id >= Main.chest.Length)
		{
			return;
		}
		try
		{
			Chest chest = Main.chest[id];
			if (chest != null && chest.x == X && chest.y == Y)
			{
				RemoveChest(id);
				if (Main.player[Main.myPlayer].chest == id)
				{
					Main.player[Main.myPlayer].chest = -1;
				}
			}
		}
		catch
		{
		}
	}

	public void AddItemToShop(Item newItem)
	{
		int num = Main.shopSellbackHelper.Remove(newItem);
		if (num >= newItem.stack)
		{
			return;
		}
		int num2 = newItem.stack - num;
		for (int i = 0; i < 39; i++)
		{
			if (num2 <= 0)
			{
				break;
			}
			if (item[i] != null && item[i].buyOnce && Item.CanStack(item[i], newItem))
			{
				int num3 = Math.Min(num2, item[i].maxStack - item[i].stack);
				item[i].stack += num3;
				num2 -= num3;
			}
		}
		for (int j = 0; j < 39; j++)
		{
			if (num2 <= 0)
			{
				break;
			}
			if (item[j] == null || item[j].IsAir)
			{
				item[j] = newItem.Clone();
				item[j].favorited = false;
				item[j].buyOnce = true;
				item[j].stack = num2;
				break;
			}
		}
	}

	public static void SetupTravelShop_AddToShop(int itemID, ref int added, ref int count)
	{
		if (itemID == 0)
		{
			return;
		}
		added++;
		Main.travelShop[count] = itemID;
		count++;
		if (itemID == 2260)
		{
			Main.travelShop[count] = 2261;
			count++;
			Main.travelShop[count] = 2262;
			count++;
		}
		if (itemID == 5680)
		{
			Main.travelShop[count] = 5681;
			count++;
			Main.travelShop[count] = 5682;
			count++;
		}
		if (itemID == 4555)
		{
			Main.travelShop[count] = 4556;
			count++;
			Main.travelShop[count] = 4557;
			count++;
		}
		if (itemID == 4321)
		{
			Main.travelShop[count] = 4322;
			count++;
		}
		if (itemID == 4323)
		{
			Main.travelShop[count] = 4324;
			count++;
			Main.travelShop[count] = 4365;
			count++;
		}
		if (itemID == 5390)
		{
			Main.travelShop[count] = 5386;
			count++;
			Main.travelShop[count] = 5387;
			count++;
		}
		if (itemID == 4666)
		{
			Main.travelShop[count] = 4664;
			count++;
			Main.travelShop[count] = 4665;
			count++;
		}
		if (itemID == 3637)
		{
			count--;
			switch (Main.rand.Next(6))
			{
			case 0:
				Main.travelShop[count++] = 3637;
				Main.travelShop[count++] = 3642;
				break;
			case 1:
				Main.travelShop[count++] = 3621;
				Main.travelShop[count++] = 3622;
				break;
			case 2:
				Main.travelShop[count++] = 3634;
				Main.travelShop[count++] = 3639;
				break;
			case 3:
				Main.travelShop[count++] = 3633;
				Main.travelShop[count++] = 3638;
				break;
			case 4:
				Main.travelShop[count++] = 3635;
				Main.travelShop[count++] = 3640;
				break;
			case 5:
				Main.travelShop[count++] = 3636;
				Main.travelShop[count++] = 3641;
				break;
			}
		}
	}

	public static bool SetupTravelShop_CanAddItemToShop(int it)
	{
		if (it == 0)
		{
			return false;
		}
		for (int i = 0; i < Main.travelShop.Length; i++)
		{
			if (Main.travelShop[i] == it)
			{
				return false;
			}
			if (it == 3637)
			{
				int num = Main.travelShop[i];
				if ((uint)(num - 3621) <= 1u || (uint)(num - 3633) <= 9u)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static void SetupTravelShop_GetPainting(Player playerWithHighestLuck, int[] rarity, ref int it, int minimumRarity = 0)
	{
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5121;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5122;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5124;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5123;
		}
		if (minimumRarity > 2)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
		{
			it = 3596;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
		{
			it = 2865;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
		{
			it = 2866;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
		{
			it = 2867;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3055;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3056;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3057;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3058;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3059;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
		{
			it = 5243;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5530;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5633;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5636;
		}
		if (minimumRarity <= 1)
		{
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5121;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5122;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5124;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5123;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5225;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5229;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5232;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5389;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5233;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5241;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5244;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5487;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5242;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5531;
			}
		}
	}

	public static void SetupTravelShop_AdjustSlotRarities(int slotItemAttempts, ref int[] rarity)
	{
		if (rarity[5] > 1 && slotItemAttempts > 4700)
		{
			rarity[5] = 1;
		}
		if (rarity[4] > 1 && slotItemAttempts > 4600)
		{
			rarity[4] = 1;
		}
		if (rarity[3] > 1 && slotItemAttempts > 4500)
		{
			rarity[3] = 1;
		}
		if (rarity[2] > 1 && slotItemAttempts > 4400)
		{
			rarity[2] = 1;
		}
		if (rarity[1] > 1 && slotItemAttempts > 4300)
		{
			rarity[1] = 1;
		}
		if (rarity[0] > 1 && slotItemAttempts > 4200)
		{
			rarity[0] = 1;
		}
	}

	public static void SetupTravelShop_GetItem(Player playerWithHighestLuck, int[] rarity, ref int it, int minimumRarity = 0)
	{
		if (minimumRarity <= 4 && playerWithHighestLuck.RollLuck(rarity[4]) == 0)
		{
			it = 3309;
		}
		if (minimumRarity <= 3 && playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 3314;
		}
		if (playerWithHighestLuck.RollLuck(rarity[5]) == 0)
		{
			it = 1987;
		}
		if (minimumRarity > 4)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0 && Main.hardMode)
		{
			it = 2270;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0 && Main.hardMode)
		{
			it = 4760;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0)
		{
			it = 2278;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0)
		{
			it = 2271;
		}
		if (minimumRarity > 3)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
		{
			it = 2223;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2272;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2276;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2284;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2285;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2286;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2287;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4744;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && NPC.downedBoss3)
		{
			it = 2296;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 3628;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && Main.hardMode)
		{
			it = 4091;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4603;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4604;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 5297;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4605;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4550;
		}
		if (minimumRarity > 2)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5680;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2268;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && WorldGen.shadowOrbSmashed)
		{
			it = 2269;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 1988;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2275;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2279;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2277;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4555;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4321;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4323;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5390;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4549;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4561;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4774;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5136;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5305;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4562;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4558;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4559;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4563;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4666;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && (NPC.downedDeerclops || NPC.downedSlimeKing || NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee || Main.hardMode))
		{
			it = 4347;
			if (Main.hardMode)
			{
				it = 4348;
			}
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedBoss1)
		{
			it = 3262;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedMechBossAny)
		{
			it = 3284;
		}
		if (minimumRarity > 1)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 5600;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2267;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2214;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2215;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2216;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2217;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 3624;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2273;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2274;
		}
		if (minimumRarity <= 0)
		{
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2266;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2281 + Main.rand.Next(3);
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2258;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2242;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2260;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3637;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 4420;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3119;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3118;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3099;
			}
		}
	}

	public static void SetupTravelShop()
	{
		for (int i = 0; i < Main.travelShop.Length; i++)
		{
			Main.travelShop[i] = 0;
		}
		Player playerWithHighestLuck = Player.GetPlayerWithHighestLuck();
		int num = Main.rand.Next(4, 7);
		if (playerWithHighestLuck.RollLuck(4) == 0)
		{
			num++;
		}
		if (playerWithHighestLuck.RollLuck(8) == 0)
		{
			num++;
		}
		if (playerWithHighestLuck.RollLuck(16) == 0)
		{
			num++;
		}
		if (playerWithHighestLuck.RollLuck(32) == 0)
		{
			num++;
		}
		if (Main.expertMode && playerWithHighestLuck.RollLuck(2) == 0)
		{
			num++;
		}
		if (NPC.peddlersSatchelWasUsed)
		{
			num++;
		}
		if (Main.tenthAnniversaryWorld)
		{
			if (!Main.getGoodWorld)
			{
				num++;
			}
			num++;
		}
		int count = 0;
		int added = 0;
		int[] array = new int[6] { 100, 200, 300, 400, 500, 600 };
		int[] rarity = array;
		int num2 = 0;
		if (Main.hardMode)
		{
			int it = 0;
			while (num2 < 5000)
			{
				num2++;
				SetupTravelShop_AdjustSlotRarities(num2, ref rarity);
				SetupTravelShop_GetItem(playerWithHighestLuck, rarity, ref it, 2);
				if (SetupTravelShop_CanAddItemToShop(it))
				{
					SetupTravelShop_AddToShop(it, ref added, ref count);
					break;
				}
			}
		}
		while (added < num)
		{
			int it2 = 0;
			SetupTravelShop_GetItem(playerWithHighestLuck, array, ref it2);
			if (SetupTravelShop_CanAddItemToShop(it2))
			{
				SetupTravelShop_AddToShop(it2, ref added, ref count);
			}
		}
		rarity = array;
		num2 = 0;
		int it3 = 0;
		while (num2 < 5000)
		{
			num2++;
			SetupTravelShop_AdjustSlotRarities(num2, ref rarity);
			SetupTravelShop_GetPainting(playerWithHighestLuck, rarity, ref it3);
			if (SetupTravelShop_CanAddItemToShop(it3))
			{
				SetupTravelShop_AddToShop(it3, ref added, ref count);
				break;
			}
		}
	}

	public void SetupShop(int type)
	{
		//IL_44e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_45ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_45dd: Unknown result type (might be due to invalid IL or missing references)
		_ = Main.LocalPlayer.currentShoppingSettings;
		Item[] array = item;
		for (int i = 0; i < maxItems; i++)
		{
			array[i] = new Item();
		}
		int num = 0;
		switch (type)
		{
		case 1:
		{
			array[num].SetDefaults(88);
			num++;
			array[num].SetDefaults(87);
			num++;
			array[num].SetDefaults(35);
			num++;
			array[num].SetDefaults(1991);
			num++;
			array[num].SetDefaults(3509);
			num++;
			array[num].SetDefaults(3506);
			num++;
			array[num].SetDefaults(8);
			num++;
			if (Main.notTheBeesWorld && !Main.remixWorld)
			{
				array[num].SetDefaults(4388);
				num++;
			}
			array[num].SetDefaults(28);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(188);
				num++;
			}
			array[num].SetDefaults(110);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(189);
				num++;
			}
			array[num].SetDefaults(40);
			num++;
			array[num].SetDefaults(42);
			num++;
			array[num].SetDefaults(965);
			num++;
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num].SetDefaults(967);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneJungle || (Main.tenthAnniversaryWorld && Main.notTheBeesWorld && !Main.remixWorld))
			{
				array[num].SetDefaults(33);
				num++;
			}
			if (Main.dayTime && Main.IsItAHappyWindyDay)
			{
				array[num++].SetDefaults(4074);
			}
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(279);
				num++;
			}
			if (!Main.dayTime)
			{
				array[num++].SetDefaults(282);
			}
			if (BirthdayParty.PartyIsUp)
			{
				array[num++].SetDefaults(5643);
			}
			if (NPC.downedBoss3)
			{
				array[num].SetDefaults(346);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(488);
				num++;
			}
			for (int l = 0; l < 58; l++)
			{
				if (Main.player[Main.myPlayer].inventory[l].type == 930)
				{
					array[num].SetDefaults(931);
					num++;
					array[num].SetDefaults(1614);
					num++;
					break;
				}
			}
			array[num].SetDefaults(1786);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(1348);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(3198);
				num++;
			}
			if (NPC.downedBoss2 || NPC.downedBoss3 || Main.hardMode)
			{
				array[num++].SetDefaults(4063);
				array[num++].SetDefaults(4673);
			}
			if (Main.player[Main.myPlayer].HasItem(3107))
			{
				array[num].SetDefaults(3108);
				num++;
			}
			break;
		}
		case 2:
			array[num].SetDefaults(97);
			num++;
			if (Main.bloodMoon || Main.hardMode)
			{
				if (WorldGen.SavedOreTiers.Silver == 168)
				{
					array[num].SetDefaults(4915);
					num++;
				}
				else
				{
					array[num].SetDefaults(278);
					num++;
				}
			}
			if ((NPC.downedBoss2 && !Main.dayTime) || Main.hardMode)
			{
				array[num].SetDefaults(47);
				num++;
			}
			array[num].SetDefaults(95);
			num++;
			array[num].SetDefaults(98);
			num++;
			if (Main.player[Main.myPlayer].ZoneGraveyard && NPC.downedBoss3)
			{
				array[num++].SetDefaults(4703);
			}
			if (!Main.dayTime)
			{
				array[num].SetDefaults(324);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(534);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1432);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(2177);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1258))
			{
				array[num].SetDefaults(1261);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1835))
			{
				array[num].SetDefaults(1836);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(3107))
			{
				array[num].SetDefaults(3108);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1782))
			{
				array[num].SetDefaults(1783);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1784))
			{
				array[num].SetDefaults(1785);
				num++;
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1736);
				num++;
				array[num].SetDefaults(1737);
				num++;
				array[num].SetDefaults(1738);
				num++;
			}
			break;
		case 3:
			if (Main.bloodMoon)
			{
				if (WorldGen.crimson)
				{
					if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
					{
						array[num].SetDefaults(2886);
						num++;
					}
					array[num].SetDefaults(2171);
					num++;
					array[num].SetDefaults(4508);
					num++;
				}
				else
				{
					if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
					{
						array[num].SetDefaults(67);
						num++;
					}
					array[num].SetDefaults(59);
					num++;
					array[num].SetDefaults(4504);
					num++;
				}
			}
			else
			{
				if (!Main.remixWorld || Main.infectedSeed || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
				{
					array[num].SetDefaults(66);
					num++;
				}
				array[num].SetDefaults(62);
				num++;
				array[num].SetDefaults(63);
				num++;
				array[num].SetDefaults(745);
				num++;
			}
			if (Main.hardMode && Main.player[Main.myPlayer].ZoneGraveyard)
			{
				if (WorldGen.crimson)
				{
					array[num].SetDefaults(59);
				}
				else
				{
					array[num].SetDefaults(2171);
				}
				num++;
			}
			array[num].SetDefaults(27);
			num++;
			array[num].SetDefaults(5309);
			num++;
			array[num].SetDefaults(114);
			num++;
			array[num].SetDefaults(1828);
			num++;
			array[num].SetDefaults(747);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(746);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(369);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(4505);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneUnderworldHeight)
			{
				array[num++].SetDefaults(5214);
			}
			else if (Main.player[Main.myPlayer].ZoneGlowshroom)
			{
				array[num++].SetDefaults(194);
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1853);
				num++;
				array[num].SetDefaults(1854);
				num++;
			}
			array[num++].SetDefaults(3215);
			array[num++].SetDefaults(3216);
			array[num++].SetDefaults(3219);
			if (WorldGen.crimson)
			{
				array[num++].SetDefaults(3218);
			}
			else
			{
				array[num++].SetDefaults(3217);
			}
			array[num++].SetDefaults(3220);
			array[num++].SetDefaults(3221);
			array[num++].SetDefaults(3222);
			array[num++].SetDefaults(4047);
			array[num++].SetDefaults(4045);
			array[num++].SetDefaults(4044);
			array[num++].SetDefaults(4043);
			array[num++].SetDefaults(4042);
			array[num++].SetDefaults(4046);
			array[num++].SetDefaults(4041);
			array[num++].SetDefaults(4241);
			array[num++].SetDefaults(4048);
			if (Main.hardMode)
			{
				switch (Main.moonPhase / 2)
				{
				case 0:
					array[num++].SetDefaults(4430);
					array[num++].SetDefaults(4431);
					array[num++].SetDefaults(4432);
					break;
				case 1:
					array[num++].SetDefaults(4433);
					array[num++].SetDefaults(4434);
					array[num++].SetDefaults(4435);
					break;
				case 2:
					array[num++].SetDefaults(4436);
					array[num++].SetDefaults(4437);
					array[num++].SetDefaults(4438);
					break;
				default:
					array[num++].SetDefaults(4439);
					array[num++].SetDefaults(4440);
					array[num++].SetDefaults(4441);
					break;
				}
			}
			else
			{
				switch (Main.moonPhase / 2)
				{
				case 0:
					array[num++].SetDefaults(4430);
					array[num++].SetDefaults(4431);
					break;
				case 1:
					array[num++].SetDefaults(4433);
					array[num++].SetDefaults(4434);
					break;
				case 2:
					array[num++].SetDefaults(4436);
					array[num++].SetDefaults(4437);
					break;
				default:
					array[num++].SetDefaults(4439);
					array[num++].SetDefaults(4440);
					break;
				}
			}
			if (!Main.hardMode && Main.vampireSeed && Main.infectedSeed)
			{
				array[num++].SetDefaults(8);
				if (WorldGen.crimson)
				{
					array[num++].SetDefaults(4386);
				}
				else
				{
					array[num++].SetDefaults(4385);
				}
			}
			break;
		case 4:
		{
			array[num].SetDefaults(168);
			num++;
			array[num].SetDefaults(166);
			num++;
			if ((NPC.downedBoss1 || NPC.downedSlimeKing) && !Main.dayTime)
			{
				array[num].SetDefaults(5542);
				num++;
			}
			array[num].SetDefaults(167);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(265);
				num++;
			}
			array[num++].SetDefaults(5481);
			array[num++].SetDefaults(5464);
			if (Main.hardMode && NPC.downedPlantBoss && NPC.downedPirates)
			{
				array[num].SetDefaults(937);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1347);
				num++;
			}
			for (int n = 0; n < 58; n++)
			{
				if (Main.player[Main.myPlayer].inventory[n].type == 4827)
				{
					array[num].SetDefaults(4827);
					num++;
					break;
				}
			}
			for (int num3 = 0; num3 < 58; num3++)
			{
				if (Main.player[Main.myPlayer].inventory[num3].type == 4824)
				{
					array[num].SetDefaults(4824);
					num++;
					break;
				}
			}
			for (int num4 = 0; num4 < 58; num4++)
			{
				if (Main.player[Main.myPlayer].inventory[num4].type == 4825)
				{
					array[num].SetDefaults(4825);
					num++;
					break;
				}
			}
			for (int num5 = 0; num5 < 58; num5++)
			{
				if (Main.player[Main.myPlayer].inventory[num5].type == 4826)
				{
					array[num].SetDefaults(4826);
					num++;
					break;
				}
			}
			break;
		}
		case 5:
			array[num].SetDefaults(254);
			num++;
			array[num].SetDefaults(981);
			num++;
			if (Main.dayTime)
			{
				array[num].SetDefaults(242);
				num++;
			}
			if (Main.moonPhase == 0)
			{
				array[num].SetDefaults(245);
				num++;
				array[num].SetDefaults(246);
				num++;
				if (!Main.dayTime)
				{
					array[num++].SetDefaults(1288);
					array[num++].SetDefaults(1289);
				}
			}
			else if (Main.moonPhase == 1)
			{
				array[num].SetDefaults(325);
				num++;
				array[num].SetDefaults(326);
				num++;
			}
			array[num].SetDefaults(269);
			num++;
			array[num].SetDefaults(270);
			num++;
			array[num].SetDefaults(271);
			num++;
			if (NPC.downedClown)
			{
				array[num].SetDefaults(503);
				num++;
				array[num].SetDefaults(504);
				num++;
				array[num].SetDefaults(505);
				num++;
			}
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(322);
				num++;
				if (!Main.dayTime)
				{
					array[num++].SetDefaults(3362);
					array[num++].SetDefaults(3363);
				}
			}
			if (NPC.downedAncientCultist)
			{
				if (Main.dayTime)
				{
					array[num++].SetDefaults(2856);
					array[num++].SetDefaults(2858);
				}
				else
				{
					array[num++].SetDefaults(2857);
					array[num++].SetDefaults(2859);
				}
			}
			if (NPC.AnyNPCs(441))
			{
				array[num++].SetDefaults(3242);
				array[num++].SetDefaults(3243);
				array[num++].SetDefaults(3244);
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(4685);
				array[num++].SetDefaults(4686);
				array[num++].SetDefaults(4704);
				array[num++].SetDefaults(4705);
				array[num++].SetDefaults(4706);
				array[num++].SetDefaults(4707);
				array[num++].SetDefaults(4708);
				array[num++].SetDefaults(4709);
			}
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num].SetDefaults(1429);
				num++;
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1740);
				num++;
			}
			if (Main.hardMode)
			{
				if (Main.moonPhase == 2)
				{
					array[num].SetDefaults(869);
					num++;
				}
				if (Main.moonPhase == 3)
				{
					array[num].SetDefaults(4994);
					num++;
					array[num].SetDefaults(4997);
					num++;
				}
				if (Main.moonPhase == 4)
				{
					array[num].SetDefaults(864);
					num++;
					array[num].SetDefaults(865);
					num++;
				}
				if (Main.moonPhase == 5)
				{
					array[num].SetDefaults(4995);
					num++;
					array[num].SetDefaults(4998);
					num++;
				}
				if (Main.moonPhase == 6)
				{
					array[num].SetDefaults(873);
					num++;
					array[num].SetDefaults(874);
					num++;
					array[num].SetDefaults(875);
					num++;
				}
				if (Main.moonPhase == 7)
				{
					array[num].SetDefaults(4996);
					num++;
					array[num].SetDefaults(4999);
					num++;
				}
			}
			if (NPC.downedFrost)
			{
				if (Main.dayTime)
				{
					array[num].SetDefaults(1275);
					num++;
				}
				else
				{
					array[num].SetDefaults(1276);
					num++;
				}
			}
			if (Main.halloween)
			{
				array[num++].SetDefaults(3246);
				array[num++].SetDefaults(3247);
			}
			if (BirthdayParty.PartyIsUp)
			{
				array[num++].SetDefaults(3730);
				array[num++].SetDefaults(3731);
				array[num++].SetDefaults(3733);
				array[num++].SetDefaults(3734);
				array[num++].SetDefaults(3735);
			}
			if (num < 38 && Main.LocalPlayer.golferScoreAccumulated >= 2000)
			{
				array[num++].SetDefaults(4744);
			}
			array[num++].SetDefaults(5308);
			if (num < 38)
			{
				array[num++].SetDefaults(5630);
			}
			break;
		case 6:
			array[num].SetDefaults(128);
			num++;
			array[num].SetDefaults(486);
			num++;
			array[num].SetDefaults(398);
			num++;
			array[num].SetDefaults(84);
			num++;
			array[num].SetDefaults(407);
			num++;
			array[num].SetDefaults(161);
			num++;
			array[num++].SetDefaults(5324);
			break;
		case 7:
			array[num].SetDefaults(487);
			num++;
			array[num].SetDefaults(496);
			num++;
			array[num].SetDefaults(500);
			num++;
			array[num].SetDefaults(507);
			num++;
			array[num].SetDefaults(508);
			num++;
			array[num].SetDefaults(531);
			num++;
			array[num].SetDefaults(149);
			num++;
			array[num].SetDefaults(576);
			num++;
			array[num].SetDefaults(3186);
			num++;
			if (Main.hardMode && Main.bloodMoon)
			{
				array[num++].SetDefaults(5461);
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1739);
				num++;
			}
			break;
		case 8:
			array[num].SetDefaults(509);
			num++;
			array[num].SetDefaults(850);
			num++;
			array[num].SetDefaults(851);
			num++;
			array[num].SetDefaults(3612);
			num++;
			array[num].SetDefaults(510);
			num++;
			array[num].SetDefaults(530);
			num++;
			array[num].SetDefaults(513);
			num++;
			array[num].SetDefaults(538);
			num++;
			array[num].SetDefaults(529);
			num++;
			array[num].SetDefaults(541);
			num++;
			array[num].SetDefaults(542);
			num++;
			array[num].SetDefaults(543);
			num++;
			array[num].SetDefaults(852);
			num++;
			array[num].SetDefaults(853);
			num++;
			array[num++].SetDefaults(4261);
			array[num++].SetDefaults(3707);
			array[num].SetDefaults(2739);
			num++;
			array[num].SetDefaults(849);
			num++;
			array[num++].SetDefaults(1263);
			array[num++].SetDefaults(3616);
			array[num++].SetDefaults(3725);
			array[num++].SetDefaults(2799);
			array[num++].SetDefaults(3619);
			array[num++].SetDefaults(3627);
			array[num++].SetDefaults(3629);
			array[num++].SetDefaults(585);
			array[num++].SetDefaults(584);
			array[num++].SetDefaults(583);
			array[num++].SetDefaults(4484);
			array[num++].SetDefaults(4485);
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4409);
				num++;
			}
			if (NPC.AnyNPCs(369) && (Main.moonPhase == 1 || Main.moonPhase == 3 || Main.moonPhase == 5 || Main.moonPhase == 7))
			{
				array[num].SetDefaults(2295);
				num++;
			}
			break;
		case 9:
		{
			array[num].SetDefaults(588);
			num++;
			array[num].SetDefaults(589);
			num++;
			array[num].SetDefaults(590);
			num++;
			array[num].SetDefaults(597);
			num++;
			array[num].SetDefaults(598);
			num++;
			array[num].SetDefaults(596);
			num++;
			for (int m = 1873; m < 1906; m++)
			{
				array[num].SetDefaults(m);
				num++;
			}
			break;
		}
		case 10:
			if (NPC.downedMechBossAny)
			{
				array[num].SetDefaults(756);
				num++;
				array[num].SetDefaults(787);
				num++;
			}
			array[num].SetDefaults(868);
			num++;
			if (NPC.downedPlantBoss)
			{
				array[num].SetDefaults(1551);
				num++;
			}
			array[num].SetDefaults(1181);
			num++;
			array[num++].SetDefaults(5231);
			if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
			{
				array[num++].SetDefaults(783);
			}
			break;
		case 11:
		{
			if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
			{
				array[num++].SetDefaults(779);
			}
			if (Main.moonPhase >= 4 && Main.hardMode)
			{
				array[num++].SetDefaults(748);
			}
			else
			{
				array[num++].SetDefaults(839);
				array[num++].SetDefaults(840);
				array[num++].SetDefaults(841);
			}
			if (NPC.downedGolemBoss)
			{
				array[num++].SetDefaults(948);
			}
			if (Main.hardMode)
			{
				array[num++].SetDefaults(3623);
			}
			array[num++].SetDefaults(3603);
			array[num++].SetDefaults(3604);
			array[num++].SetDefaults(3607);
			array[num++].SetDefaults(3605);
			array[num++].SetDefaults(3606);
			array[num++].SetDefaults(3608);
			array[num++].SetDefaults(3618);
			array[num++].SetDefaults(3602);
			array[num++].SetDefaults(3663);
			array[num++].SetDefaults(3609);
			array[num++].SetDefaults(3610);
			if (Main.hardMode || !Main.getGoodWorld)
			{
				array[num++].SetDefaults(995);
			}
			if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
			{
				array[num++].SetDefaults(2203);
			}
			if (WorldGen.crimson)
			{
				array[num++].SetDefaults(2193);
			}
			else
			{
				array[num++].SetDefaults(4142);
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(2192);
			}
			bool zoneJungle = Main.player[Main.myPlayer].ZoneJungle;
			if (zoneJungle)
			{
				array[num++].SetDefaults(2204);
			}
			if (zoneJungle && NPC.downedGolemBoss)
			{
				array[num++].SetDefaults(2195);
			}
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num++].SetDefaults(2198);
			}
			if (Main.player[Main.myPlayer].ZoneSkyHeight)
			{
				array[num++].SetDefaults(2197);
			}
			if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
			{
				if (Main.eclipse || Main.bloodMoon)
				{
					if (WorldGen.crimson)
					{
						array[num++].SetDefaults(784);
					}
					else
					{
						array[num++].SetDefaults(782);
					}
				}
				else if (Main.player[Main.myPlayer].ZoneHallow)
				{
					array[num++].SetDefaults(781);
				}
				else
				{
					array[num++].SetDefaults(780);
				}
				if (NPC.downedMoonlord)
				{
					array[num++].SetDefaults(5392);
					array[num++].SetDefaults(5393);
					array[num++].SetDefaults(5394);
				}
			}
			if (Main.hardMode)
			{
				array[num++].SetDefaults(1344);
				array[num++].SetDefaults(4472);
			}
			if (Main.halloween)
			{
				array[num++].SetDefaults(1742);
			}
			break;
		}
		case 12:
			array[num++].SetDefaults(1120);
			array[num++].SetDefaults(5920);
			if (Main.halloween)
			{
				array[num++].SetDefaults(3248);
				array[num++].SetDefaults(1741);
			}
			array[num++].SetDefaults(1037);
			array[num++].SetDefaults(2874);
			if (Main.netMode == 1)
			{
				array[num++].SetDefaults(1969);
			}
			if (Main.moonPhase == 0)
			{
				array[num++].SetDefaults(2871);
				array[num++].SetDefaults(2872);
			}
			if (!Main.dayTime && Main.bloodMoon)
			{
				array[num++].SetDefaults(4663);
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(4662);
			}
			break;
		case 13:
			array[num].SetDefaults(859);
			num++;
			if (Main.LocalPlayer.golferScoreAccumulated >= 500)
			{
				array[num++].SetDefaults(4743);
			}
			array[num].SetDefaults(1000);
			num++;
			array[num].SetDefaults(1168);
			num++;
			if (Main.dayTime)
			{
				array[num].SetDefaults(1449);
				num++;
			}
			else
			{
				array[num].SetDefaults(4552);
				num++;
			}
			array[num].SetDefaults(1345);
			num++;
			array[num].SetDefaults(1450);
			num++;
			array[num++].SetDefaults(3253);
			array[num++].SetDefaults(4553);
			array[num++].SetDefaults(2700);
			array[num++].SetDefaults(2738);
			array[num++].SetDefaults(4470);
			array[num++].SetDefaults(4681);
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(4682);
			}
			if (LanternNight.LanternsUp)
			{
				array[num++].SetDefaults(4702);
			}
			if (Main.player[Main.myPlayer].HasItem(3548))
			{
				array[num].SetDefaults(3548);
				num++;
			}
			if (NPC.AnyNPCs(229))
			{
				array[num++].SetDefaults(3369);
			}
			if (NPC.downedGolemBoss)
			{
				array[num++].SetDefaults(3546);
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(3214);
				num++;
				array[num].SetDefaults(2868);
				num++;
				array[num].SetDefaults(970);
				num++;
				array[num].SetDefaults(971);
				num++;
				array[num].SetDefaults(972);
				num++;
				array[num].SetDefaults(973);
				num++;
			}
			array[num++].SetDefaults(4791);
			array[num++].SetDefaults(3747);
			array[num++].SetDefaults(3732);
			array[num++].SetDefaults(3742);
			if (BirthdayParty.PartyIsUp)
			{
				array[num++].SetDefaults(3749);
				array[num++].SetDefaults(3746);
				array[num++].SetDefaults(3739);
				array[num++].SetDefaults(3740);
				array[num++].SetDefaults(3741);
				array[num++].SetDefaults(3737);
				array[num++].SetDefaults(3738);
				array[num++].SetDefaults(3736);
				array[num++].SetDefaults(3745);
				array[num++].SetDefaults(3744);
				array[num++].SetDefaults(3743);
			}
			break;
		case 14:
			array[num].SetDefaults(771);
			num++;
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(772);
				num++;
			}
			if (!Main.dayTime || Main.eclipse)
			{
				array[num].SetDefaults(773);
				num++;
			}
			if (Main.eclipse)
			{
				array[num].SetDefaults(774);
				num++;
			}
			if (NPC.downedMartians)
			{
				array[num++].SetDefaults(4445);
				if (Main.bloodMoon || Main.eclipse)
				{
					array[num++].SetDefaults(4446);
				}
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(4459);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(760);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1346);
				num++;
			}
			if (Main.hardMode)
			{
				array[num++].SetDefaults(5452);
				array[num++].SetDefaults(5451);
				array[num++].SetDefaults(5738);
			}
			array[num].SetDefaults(5598);
			num++;
			array[num].SetDefaults(5599);
			num++;
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4409);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4392);
				num++;
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1743);
				num++;
				array[num].SetDefaults(1744);
				num++;
				array[num].SetDefaults(1745);
				num++;
			}
			if (NPC.downedMartians)
			{
				array[num++].SetDefaults(2862);
				array[num++].SetDefaults(3109);
			}
			if (Main.player[Main.myPlayer].HasItem(3384) || Main.player[Main.myPlayer].HasItem(3664))
			{
				array[num].SetDefaults(3664);
				num++;
			}
			array[num].SetDefaults(5928);
			num++;
			break;
		case 15:
		{
			array[num].SetDefaults(1071);
			num++;
			array[num].SetDefaults(1072);
			num++;
			array[num].SetDefaults(1100);
			num++;
			for (int num6 = 1073; num6 <= 1084; num6++)
			{
				array[num].SetDefaults(num6);
				num++;
			}
			array[num].SetDefaults(1097);
			num++;
			array[num].SetDefaults(1099);
			num++;
			array[num].SetDefaults(1098);
			num++;
			array[num].SetDefaults(1966);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(1967);
				num++;
				array[num].SetDefaults(1968);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4668);
				num++;
				if (NPC.downedPlantBoss || NPC.AnyNPCs(124))
				{
					array[num].SetDefaults(5344);
					num++;
				}
			}
			break;
		}
		case 25:
		{
			if (Main.xMas)
			{
				int num8 = 1948;
				while (num8 <= 1957 && num < 39)
				{
					array[num].SetDefaults(num8);
					num8++;
					num++;
				}
			}
			int num9 = 2158;
			while (num9 <= 2160 && num < 39)
			{
				array[num].SetDefaults(num9);
				num9++;
				num++;
			}
			int num10 = 2008;
			while (num10 <= 2014 && num < 39)
			{
				array[num].SetDefaults(num10);
				num10++;
				num++;
			}
			if (!Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(1490);
				num++;
				if (Main.moonPhase <= 1)
				{
					array[num].SetDefaults(1481);
					num++;
				}
				else if (Main.moonPhase <= 3)
				{
					array[num].SetDefaults(1482);
					num++;
				}
				else if (Main.moonPhase <= 5)
				{
					array[num].SetDefaults(1483);
					num++;
				}
				else
				{
					array[num].SetDefaults(1484);
					num++;
				}
			}
			if (Main.player[Main.myPlayer].ShoppingZone_Forest)
			{
				array[num].SetDefaults(5245);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneCrimson)
			{
				array[num].SetDefaults(1492);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneCorrupt)
			{
				array[num].SetDefaults(1488);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneHallow)
			{
				array[num].SetDefaults(1489);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneJungle)
			{
				array[num].SetDefaults(1486);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num].SetDefaults(5491);
				num++;
				array[num].SetDefaults(1487);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneDesert)
			{
				array[num].SetDefaults(1491);
				num++;
			}
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(1493);
				num++;
			}
			if (!Main.player[Main.myPlayer].ZoneGraveyard)
			{
				if (Main.player[Main.myPlayer].ZoneSkyHeight)
				{
					array[num].SetDefaults(1485);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneSkyHeight && Main.hardMode)
				{
					array[num].SetDefaults(1494);
					num++;
				}
			}
			if (Main.IsItStorming)
			{
				array[num].SetDefaults(5251);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4723);
				num++;
				array[num].SetDefaults(4724);
				num++;
				array[num].SetDefaults(4725);
				num++;
				array[num].SetDefaults(4726);
				num++;
				array[num].SetDefaults(4727);
				num++;
				array[num].SetDefaults(5257);
				num++;
				array[num].SetDefaults(4728);
				num++;
				array[num].SetDefaults(4729);
				num++;
			}
			break;
		}
		case 16:
			array[num++].SetDefaults(1430);
			array[num++].SetDefaults(986);
			if (NPC.AnyNPCs(108))
			{
				array[num++].SetDefaults(2999);
			}
			if (Main.player[Main.myPlayer].ZoneJungle)
			{
				array[num++].SetDefaults(6147);
			}
			if (!Main.dayTime)
			{
				array[num++].SetDefaults(1158);
			}
			if (Main.hardMode && NPC.downedPlantBoss)
			{
				array[num++].SetDefaults(1159);
				array[num++].SetDefaults(1160);
				array[num++].SetDefaults(1161);
				if (Main.player[Main.myPlayer].ZoneJungle)
				{
					array[num++].SetDefaults(1167);
				}
				array[num++].SetDefaults(1339);
			}
			if (Main.hardMode && Main.player[Main.myPlayer].ZoneJungle)
			{
				array[num++].SetDefaults(1171);
				if (!Main.dayTime && NPC.downedPlantBoss)
				{
					array[num++].SetDefaults(1162);
				}
			}
			array[num++].SetDefaults(909);
			array[num++].SetDefaults(910);
			array[num++].SetDefaults(940);
			array[num++].SetDefaults(941);
			array[num++].SetDefaults(942);
			array[num++].SetDefaults(943);
			array[num++].SetDefaults(944);
			array[num++].SetDefaults(945);
			array[num++].SetDefaults(4922);
			array[num++].SetDefaults(4417);
			if (Main.player[Main.myPlayer].HasItem(1835))
			{
				array[num++].SetDefaults(1836);
			}
			if (Main.player[Main.myPlayer].HasItem(1258))
			{
				array[num++].SetDefaults(1261);
			}
			if (Main.halloween)
			{
				array[num++].SetDefaults(1791);
			}
			break;
		case 17:
			array[num].SetDefaults(928);
			num++;
			array[num].SetDefaults(929);
			num++;
			array[num].SetDefaults(876);
			num++;
			array[num].SetDefaults(877);
			num++;
			array[num].SetDefaults(878);
			num++;
			array[num++].SetDefaults(2434);
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(5926);
			}
			if (Main.player[Main.myPlayer].ZoneBeach)
			{
				array[num++].SetDefaults(1180);
			}
			if (Main.hardMode && NPC.downedMechBossAny && NPC.AnyNPCs(208))
			{
				array[num].SetDefaults(1337);
				num++;
			}
			break;
		case 18:
		{
			array[num].SetDefaults(1990);
			num++;
			array[num].SetDefaults(1979);
			num++;
			if (Main.player[Main.myPlayer].statLifeMax >= 400)
			{
				array[num].SetDefaults(1977);
				num++;
			}
			if (Main.player[Main.myPlayer].statManaMax >= 200)
			{
				array[num].SetDefaults(1978);
				num++;
			}
			long num2 = 0L;
			for (int k = 0; k < 54; k++)
			{
				if (Main.player[Main.myPlayer].inventory[k].type == 71)
				{
					num2 += Main.player[Main.myPlayer].inventory[k].stack;
				}
				if (Main.player[Main.myPlayer].inventory[k].type == 72)
				{
					num2 += (long)Main.player[Main.myPlayer].inventory[k].stack * 100L;
				}
				if (Main.player[Main.myPlayer].inventory[k].type == 73)
				{
					num2 += (long)Main.player[Main.myPlayer].inventory[k].stack * 10000L;
				}
				if (Main.player[Main.myPlayer].inventory[k].type == 74)
				{
					num2 += (long)Main.player[Main.myPlayer].inventory[k].stack * 1000000L;
				}
				if (num2 < 0 || num2 > 9999999999L)
				{
					num2 = 9999999999L;
					break;
				}
			}
			if (num2 < 0 || num2 > 9999999999L)
			{
				num2 = 9999999999L;
			}
			if (num2 >= 1000000)
			{
				array[num++].SetDefaults(1980);
			}
			if ((Main.moonPhase % 2 == 0 && Main.dayTime) || (Main.moonPhase % 2 == 1 && !Main.dayTime))
			{
				array[num].SetDefaults(1981);
				num++;
			}
			if (Main.player[Main.myPlayer].team != 0 && Main.netMode == 1)
			{
				array[num].SetDefaults(1982);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1983);
				num++;
			}
			if (NPC.AnyNPCs(208))
			{
				array[num].SetDefaults(1984);
				num++;
			}
			if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				array[num].SetDefaults(1985);
				num++;
			}
			if (Main.hardMode && NPC.downedMechBossAny)
			{
				array[num].SetDefaults(1986);
				num++;
			}
			if (Main.hardMode && NPC.downedMartians)
			{
				array[num].SetDefaults(2863);
				num++;
				array[num].SetDefaults(3259);
				num++;
			}
			array[num++].SetDefaults(5104);
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(5577);
			}
			break;
		}
		case 19:
		{
			Player localPlayer = Main.LocalPlayer;
			if (localPlayer.HasItemInAnyInventory(5667) || localPlayer.HasItemInAnyInventory(5663) || localPlayer.HasItemInAnyInventory(5664) || localPlayer.HasItemInAnyInventory(5665) || localPlayer.HasItemInAnyInventory(5666) || localPlayer.HasItemInAnyInventory(6174) || localPlayer.HasItemInAnyInventory(6148) || localPlayer.HasItemInAnyInventory(6149) || localPlayer.HasItemInAnyInventory(6150) || localPlayer.HasItemInAnyInventory(6151))
			{
				array[num].SetDefaults(5735);
				num++;
				array[num].SetDefaults(5736);
				num++;
			}
			for (int j = 0; j < maxItems; j++)
			{
				if (Main.travelShop[j] != 0)
				{
					array[num].SetDefaults(Main.travelShop[j]);
					num++;
				}
			}
			break;
		}
		case 20:
			if (Main.moonPhase == 0)
			{
				array[num].SetDefaults(284);
				num++;
			}
			if (Main.moonPhase == 1)
			{
				array[num].SetDefaults(946);
				num++;
			}
			if (Main.moonPhase == 2 && !Main.remixWorld)
			{
				array[num].SetDefaults(3069);
				num++;
			}
			if (Main.moonPhase == 2 && Main.remixWorld)
			{
				array[num].SetDefaults(517);
				num++;
			}
			if (Main.moonPhase == 3)
			{
				array[num].SetDefaults(4341);
				num++;
			}
			if (Main.moonPhase == 4)
			{
				array[num].SetDefaults(285);
				num++;
			}
			if (Main.moonPhase == 5)
			{
				array[num].SetDefaults(953);
				num++;
			}
			if (Main.moonPhase == 6)
			{
				array[num].SetDefaults(3068);
				num++;
			}
			if (Main.moonPhase == 7)
			{
				array[num].SetDefaults(3084);
				num++;
			}
			if (Main.moonPhase % 2 == 0)
			{
				array[num].SetDefaults(3001);
				num++;
			}
			if (Main.moonPhase % 2 != 0)
			{
				array[num].SetDefaults(28);
				num++;
			}
			if (Main.moonPhase % 2 != 0 && Main.hardMode)
			{
				array[num].SetDefaults(188);
				num++;
			}
			if (!Main.dayTime || Main.moonPhase == 0)
			{
				array[num].SetDefaults(3002);
				num++;
				if (Main.player[Main.myPlayer].HasItem(930))
				{
					array[num].SetDefaults(5377);
					num++;
				}
			}
			else if (Main.dayTime && Main.moonPhase != 0)
			{
				array[num].SetDefaults(282);
				num++;
			}
			if (Main.time % 60.0 * 60.0 * 6.0 <= 10800.0)
			{
				array[num].SetDefaults(3004);
			}
			else
			{
				array[num].SetDefaults(8);
			}
			num++;
			if (Main.moonPhase == 0 || Main.moonPhase == 1 || Main.moonPhase == 4 || Main.moonPhase == 5)
			{
				array[num].SetDefaults(3003);
			}
			else
			{
				array[num].SetDefaults(40);
			}
			num++;
			if (Main.moonPhase % 4 == 0)
			{
				array[num++].SetDefaults(3310);
			}
			else if (Main.moonPhase % 4 == 1)
			{
				array[num++].SetDefaults(3313);
			}
			else if (Main.moonPhase % 4 == 2)
			{
				array[num++].SetDefaults(3312);
			}
			else
			{
				array[num++].SetDefaults(3311);
			}
			if (Main.moonPhase == 1 || Main.moonPhase == 2)
			{
				array[num++].SetDefaults(5640);
			}
			else if (Main.moonPhase == 3 || Main.moonPhase == 5)
			{
				array[num++].SetDefaults(5641);
			}
			else if (Main.moonPhase == 6 || Main.moonPhase == 7)
			{
				array[num++].SetDefaults(5642);
			}
			array[num].SetDefaults(166);
			num++;
			array[num].SetDefaults(965);
			num++;
			if (Main.hardMode)
			{
				array[num++].SetDefaults(3316);
				array[num].SetDefaults(3334);
				num++;
				if (NPC.downedMechBossAny)
				{
					array[num].SetDefaults(5540);
					num++;
				}
				if (Main.bloodMoon)
				{
					array[num].SetDefaults(3258);
					num++;
				}
			}
			if (Main.moonPhase == 0 && !Main.dayTime)
			{
				array[num].SetDefaults(3043);
				num++;
			}
			if (!Main.player[Main.myPlayer].ateArtisanBread && Main.moonPhase >= 3 && Main.moonPhase <= 5)
			{
				array[num++].SetDefaults(5326);
			}
			break;
		case 21:
		{
			bool flag = Main.hardMode && NPC.downedMechBossAny;
			bool num7 = Main.hardMode && NPC.downedGolemBoss;
			array[num].SetDefaults(353);
			num++;
			array[num].SetDefaults(3828);
			if (num7)
			{
				array[num].shopCustomPrice = Item.buyPrice(0, 4);
			}
			else if (flag)
			{
				array[num].shopCustomPrice = Item.buyPrice(0, 1);
			}
			else
			{
				array[num].shopCustomPrice = Item.buyPrice(0, 0, 25);
			}
			num++;
			array[num].SetDefaults(3816);
			num++;
			array[num].SetDefaults(3813);
			array[num].shopCustomPrice = 50;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			num = 10;
			array[num].SetDefaults(3818);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			array[num].SetDefaults(3824);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			array[num].SetDefaults(3832);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			array[num].SetDefaults(3829);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			if (flag)
			{
				num = 20;
				array[num].SetDefaults(3819);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3825);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3833);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3830);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			}
			if (num7)
			{
				num = 30;
				array[num].SetDefaults(3820);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3826);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3834);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3831);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			}
			if (flag)
			{
				num = 4;
				array[num].SetDefaults(3800);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3801);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3802);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 14;
				array[num].SetDefaults(3797);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3798);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3799);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 24;
				array[num].SetDefaults(3803);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3804);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3805);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 34;
				array[num].SetDefaults(3806);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3807);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3808);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
			}
			if (num7)
			{
				num = 7;
				array[num].SetDefaults(3871);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3872);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3873);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 17;
				array[num].SetDefaults(3874);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3875);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3876);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 27;
				array[num].SetDefaults(3877);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3878);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3879);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 37;
				array[num].SetDefaults(3880);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3881);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3882);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
			}
			if (num7)
			{
				num = 40;
			}
			else
			{
				num = ((!flag) ? 4 : 30);
			}
			break;
		}
		case 22:
		{
			int golferScoreAccumulated = Main.LocalPlayer.golferScoreAccumulated;
			array[num++].SetDefaults(4587);
			array[num++].SetDefaults(4590);
			array[num++].SetDefaults(4589);
			array[num++].SetDefaults(4588);
			array[num++].SetDefaults(4083);
			array[num++].SetDefaults(4084);
			array[num++].SetDefaults(4085);
			array[num++].SetDefaults(4086);
			array[num++].SetDefaults(4087);
			array[num++].SetDefaults(4088);
			if (golferScoreAccumulated >= 500)
			{
				array[num].SetDefaults(4039);
				num++;
				array[num].SetDefaults(4094);
				num++;
				array[num].SetDefaults(4093);
				num++;
				array[num].SetDefaults(4092);
				num++;
			}
			array[num++].SetDefaults(4089);
			array[num++].SetDefaults(3989);
			array[num++].SetDefaults(4095);
			array[num++].SetDefaults(4040);
			array[num++].SetDefaults(4319);
			array[num++].SetDefaults(4320);
			if (golferScoreAccumulated > 1000)
			{
				array[num].SetDefaults(4591);
				num++;
				array[num].SetDefaults(4594);
				num++;
				array[num].SetDefaults(4593);
				num++;
				array[num].SetDefaults(4592);
				num++;
			}
			array[num++].SetDefaults(4135);
			array[num++].SetDefaults(4138);
			array[num++].SetDefaults(4136);
			array[num++].SetDefaults(4137);
			array[num++].SetDefaults(4049);
			if (golferScoreAccumulated > 500)
			{
				array[num].SetDefaults(4265);
				num++;
			}
			if (golferScoreAccumulated > 2000)
			{
				array[num].SetDefaults(4595);
				num++;
				array[num].SetDefaults(4598);
				num++;
				array[num].SetDefaults(4597);
				num++;
				array[num].SetDefaults(4596);
				num++;
				if (NPC.downedBoss3)
				{
					array[num].SetDefaults(4264);
					num++;
				}
			}
			if (golferScoreAccumulated > 500)
			{
				array[num].SetDefaults(4599);
				num++;
			}
			if (golferScoreAccumulated >= 1000)
			{
				array[num].SetDefaults(4600);
				num++;
			}
			if (golferScoreAccumulated >= 2000)
			{
				array[num].SetDefaults(4601);
				num++;
			}
			if (golferScoreAccumulated >= 2000)
			{
				if (Main.moonPhase == 0 || Main.moonPhase == 1)
				{
					array[num].SetDefaults(4658);
					num++;
				}
				else if (Main.moonPhase == 2 || Main.moonPhase == 3)
				{
					array[num].SetDefaults(4659);
					num++;
				}
				else if (Main.moonPhase == 4 || Main.moonPhase == 5)
				{
					array[num].SetDefaults(4660);
					num++;
				}
				else if (Main.moonPhase == 6 || Main.moonPhase == 7)
				{
					array[num].SetDefaults(4661);
					num++;
				}
			}
			break;
		}
		case 23:
		{
			BestiaryUnlockProgressReport bestiaryProgressReport = Main.GetBestiaryProgressReport();
			if (BestiaryGirl_IsFairyTorchAvailable())
			{
				array[num++].SetDefaults(4776);
			}
			array[num++].SetDefaults(4767);
			if (Main.moonPhase == 0 && !Main.dayTime)
			{
				array[num++].SetDefaults(5253);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.45f)
			{
				array[num++].SetDefaults(5635);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.1f)
			{
				array[num++].SetDefaults(4759);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.03f)
			{
				array[num++].SetDefaults(4672);
			}
			array[num++].SetDefaults(4829);
			if (bestiaryProgressReport.CompletionPercent >= 0.25f)
			{
				array[num++].SetDefaults(4830);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.45f)
			{
				array[num++].SetDefaults(4910);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4871);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4907);
			}
			if (NPC.downedTowerSolar)
			{
				array[num++].SetDefaults(4677);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.1f)
			{
				array[num++].SetDefaults(4676);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4762);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.25f)
			{
				array[num++].SetDefaults(4716);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4785);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4786);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4787);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f && Main.hardMode)
			{
				array[num++].SetDefaults(4788);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.25f)
			{
				array[num++].SetDefaults(4763);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.4f)
			{
				array[num++].SetDefaults(4955);
			}
			if (Main.hardMode && Main.bloodMoon)
			{
				array[num++].SetDefaults(4736);
			}
			if (NPC.downedPlantBoss)
			{
				array[num++].SetDefaults(4701);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(4765);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(4766);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(5285);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(4777);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.7f)
			{
				array[num++].SetDefaults(4735);
			}
			if (bestiaryProgressReport.CompletionPercent >= 1f)
			{
				array[num++].SetDefaults(4951);
			}
			if (BirthdayParty.PartyIsUp)
			{
				array[num++].SetDefaults(5466);
			}
			switch (Main.moonPhase)
			{
			case 0:
			case 1:
				array[num++].SetDefaults(4768);
				array[num++].SetDefaults(4769);
				break;
			case 2:
			case 3:
				array[num++].SetDefaults(4770);
				array[num++].SetDefaults(4771);
				break;
			case 4:
			case 5:
				array[num++].SetDefaults(4772);
				array[num++].SetDefaults(4773);
				break;
			case 6:
			case 7:
				array[num++].SetDefaults(4560);
				array[num++].SetDefaults(4775);
				break;
			}
			if (Main.vampireSeed && !Main.infectedSeed)
			{
				array[num++].SetDefaults(8);
			}
			break;
		}
		case 24:
			array[num++].SetDefaults(5071);
			array[num++].SetDefaults(5072);
			array[num++].SetDefaults(5073);
			array[num++].SetDefaults(5076);
			array[num++].SetDefaults(5077);
			array[num++].SetDefaults(5078);
			array[num++].SetDefaults(5079);
			array[num++].SetDefaults(5080);
			array[num++].SetDefaults(5081);
			array[num++].SetDefaults(5082);
			array[num++].SetDefaults(5083);
			array[num++].SetDefaults(5084);
			array[num++].SetDefaults(5085);
			array[num++].SetDefaults(5086);
			array[num++].SetDefaults(5087);
			array[num++].SetDefaults(5310);
			array[num++].SetDefaults(5222);
			array[num++].SetDefaults(5228);
			if (NPC.downedSlimeKing && NPC.downedQueenSlime)
			{
				array[num++].SetDefaults(5266);
			}
			if (Main.hardMode && NPC.downedMoonlord)
			{
				array[num++].SetDefaults(5044);
			}
			if (Main.tenthAnniversaryWorld)
			{
				array[num++].SetDefaults(1309);
				array[num++].SetDefaults(1859);
				array[num++].SetDefaults(1358);
				if (Main.player[Main.myPlayer].ZoneDesert)
				{
					array[num++].SetDefaults(857);
				}
				if (Main.bloodMoon)
				{
					array[num++].SetDefaults(4144);
				}
				if (Main.hardMode && NPC.downedPirates)
				{
					if (Main.moonPhase == 0 || Main.moonPhase == 1)
					{
						array[num++].SetDefaults(2584);
					}
					if (Main.moonPhase == 2 || Main.moonPhase == 3)
					{
						array[num++].SetDefaults(854);
					}
					if (Main.moonPhase == 4 || Main.moonPhase == 5)
					{
						array[num++].SetDefaults(855);
					}
					if (Main.moonPhase == 6 || Main.moonPhase == 7)
					{
						array[num++].SetDefaults(905);
					}
				}
			}
			array[num++].SetDefaults(5088);
			break;
		}
		bool num11 = type != 19 && type != 20 && type != 21;
		bool flag2 = TeleportPylonsSystem.DoesPositionHaveEnoughNPCs(2, Main.LocalPlayer.Center.ToTileCoordinates16());
		if ((num11 & flag2) && !Main.player[Main.myPlayer].ZoneCorrupt && !Main.player[Main.myPlayer].ZoneCrimson)
		{
			if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && !Main.player[Main.myPlayer].ZoneGlowshroom)
			{
				if (Main.remixWorld)
				{
					if ((double)(Main.player[Main.myPlayer].Center.Y / 16f) > Main.rockLayer && Main.player[Main.myPlayer].Center.Y / 16f < (float)(Main.maxTilesY - 350) && num < 39)
					{
						array[num++].SetDefaults(4876);
					}
				}
				else if (!Main.player[Main.myPlayer].ShoppingZone_BelowSurface && num < 39)
				{
					array[num++].SetDefaults(4876);
				}
			}
			if (Main.player[Main.myPlayer].ZoneSnow && num < 39)
			{
				array[num++].SetDefaults(4920);
			}
			if (Main.player[Main.myPlayer].ZoneDesert && num < 39)
			{
				array[num++].SetDefaults(4919);
			}
			if (Main.player[Main.myPlayer].ZoneUnderworldHeight)
			{
				if (num < 39)
				{
					array[num++].SetDefaults(5652);
				}
			}
			else if (Main.remixWorld)
			{
				if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && Main.player[Main.myPlayer].ShoppingZone_BelowSurface && num < 39)
				{
					array[num++].SetDefaults(4917);
				}
			}
			else if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && !Main.player[Main.myPlayer].ZoneGlowshroom && Main.player[Main.myPlayer].ShoppingZone_BelowSurface && num < 39)
			{
				array[num++].SetDefaults(4917);
			}
			bool flag3 = Main.player[Main.myPlayer].ZoneBeach && (double)Main.player[Main.myPlayer].position.Y < Main.worldSurface * 16.0;
			if (Main.remixWorld)
			{
				float num12 = Main.player[Main.myPlayer].position.X / 16f;
				float num13 = Main.player[Main.myPlayer].position.Y / 16f;
				flag3 |= ((double)num12 < (double)Main.maxTilesX * 0.43 || (double)num12 > (double)Main.maxTilesX * 0.57) && (double)num13 > Main.rockLayer && num13 < (float)(Main.maxTilesY - 350);
			}
			if (flag3 && num < 39)
			{
				array[num++].SetDefaults(4918);
			}
			if (Main.player[Main.myPlayer].ZoneJungle && num < 39)
			{
				array[num++].SetDefaults(4875);
			}
			if (Main.player[Main.myPlayer].ZoneHallow && num < 39)
			{
				array[num++].SetDefaults(4916);
			}
			if (Main.player[Main.myPlayer].ZoneGlowshroom && (!Main.remixWorld || !Main.player[Main.myPlayer].ZoneUnderworldHeight) && num < 39)
			{
				array[num++].SetDefaults(4921);
			}
		}
		for (int num14 = 0; num14 < num; num14++)
		{
			array[num14].isAShopItem = true;
		}
	}

	private static bool BestiaryGirl_IsFairyTorchAvailable()
	{
		if (!DidDiscoverBestiaryEntry(585))
		{
			return false;
		}
		if (!DidDiscoverBestiaryEntry(584))
		{
			return false;
		}
		if (!DidDiscoverBestiaryEntry(583))
		{
			return false;
		}
		return true;
	}

	private static bool DidDiscoverBestiaryEntry(int npcId)
	{
		return Main.BestiaryDB.FindEntryByNPCID(npcId).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
	}

	public static void AskForChestToEatItem(Vector2 worldPosition, int duration)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Point val = worldPosition.ToTileCoordinates();
		int num = FindChest(val.X, val.Y);
		if (num != -1)
		{
			Chest chest = Main.chest[num];
			chest.eatingAnimationTime = Math.Max(duration, chest.eatingAnimationTime);
		}
	}

	private static RoomCheckParticle GetNewParticle()
	{
		return new RoomCheckParticle();
	}

	public static void IndicateBlockedChest(int chestIndex)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		Chest chest = Main.chest[chestIndex];
		if (chest != null)
		{
			RoomCheckParticle roomCheckParticle = _particlePool.RequestParticle();
			roomCheckParticle.SetBasicInfo(TextureAssets.Cd, null, Vector2.Zero, new Vector2((float)(chest.x * 16 + 16), (float)(chest.y * 16 + 16)));
			float num = 60f;
			roomCheckParticle.Delay = 0;
			float num2 = num - (float)roomCheckParticle.Delay;
			roomCheckParticle.SetTypeInfo(num2, fullbright: false);
			roomCheckParticle.FadeInNormalizedTime = Utils.Remap(num - 55f, roomCheckParticle.Delay, num, 0f, 1f);
			roomCheckParticle.FadeOutNormalizedTime = Utils.Remap(num - 20f, roomCheckParticle.Delay, num, 0f, 1f);
			roomCheckParticle.ColorTint = new Color(255, 40, 40, 255);
			roomCheckParticle.Scale = Vector2.One * 1f;
			roomCheckParticle.Velocity = Vector2.UnitY * 0.1f;
			roomCheckParticle.AccelerationPerFrame = Vector2.UnitY * -0.4f / num2;
			Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle);
		}
	}

	public static void UpdateChestFrames()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		int num = 8000;
		_chestInUse.Clear();
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active && Main.player[i].chest >= 0 && Main.player[i].chest < num)
			{
				_chestInUse.Add(Main.player[i].chest);
			}
		}
		Chest chest = null;
		for (int j = 0; j < num; j++)
		{
			chest = Main.chest[j];
			if (chest != null)
			{
				if (_chestInUse.Contains(j))
				{
					chest.frameCounter++;
				}
				else
				{
					chest.frameCounter--;
				}
				if (chest.eatingAnimationTime == 9 && chest.frame == 1)
				{
					SoundEngine.PlaySound(7, new Vector2((float)(chest.x * 16 + 16), (float)(chest.y * 16 + 16)));
				}
				if (chest.eatingAnimationTime > 0)
				{
					chest.eatingAnimationTime--;
				}
				if (chest.frameCounter < chest.eatingAnimationTime)
				{
					chest.frameCounter = chest.eatingAnimationTime;
				}
				if (chest.frameCounter < 0)
				{
					chest.frameCounter = 0;
				}
				if (chest.frameCounter > 10)
				{
					chest.frameCounter = 10;
				}
				if (chest.frameCounter == 0)
				{
					chest.frame = 0;
				}
				else if (chest.frameCounter == 10)
				{
					chest.frame = 2;
				}
				else
				{
					chest.frame = 1;
				}
			}
		}
	}

	public void FixLoadedData()
	{
		Item[] array = item;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FixAgainstExploit();
		}
	}
}
