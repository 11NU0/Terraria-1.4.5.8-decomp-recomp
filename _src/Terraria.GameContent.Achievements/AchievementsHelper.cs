namespace Terraria.GameContent.Achievements;

public class AchievementsHelper
{
	public delegate void ItemPickupEvent(Player player, short itemId, int count);

	public delegate void ItemCraftEvent(short itemId, int count);

	public delegate void TileDestroyedEvent(Player player, ushort tileId);

	public delegate void NPCKilledEvent(Player player, short npcId);

	public delegate void ProgressionEventEvent(int eventID);

	private static bool _isMining;

	private static int _lastResearchVersion;

	private static bool mayhemOK;

	private static bool mayhem1down;

	private static bool mayhem2down;

	private static bool mayhem3down;

	public static bool CurrentlyMining
	{
		get
		{
			return _isMining;
		}
		set
		{
			_isMining = value;
		}
	}

	public static event ItemPickupEvent OnItemPickup;

	public static event ItemCraftEvent OnItemCraft;

	public static event TileDestroyedEvent OnTileDestroyed;

	public static event NPCKilledEvent OnNPCKilled;

	public static event ProgressionEventEvent OnProgressionEvent;

	public static void NotifyTileDestroyed(Player player, ushort tile)
	{
		if (!Main.gameMenu && _isMining && OnTileDestroyed != null)
		{
			OnTileDestroyed(player, tile);
		}
	}

	public static void NotifyItemPickup(Player player, Item item)
	{
		if (OnItemPickup != null)
		{
			OnItemPickup(player, (short)item.type, item.stack);
		}
	}

	public static void NotifyItemPickup(Player player, Item item, int customStack)
	{
		if (OnItemPickup != null)
		{
			OnItemPickup(player, (short)item.type, customStack);
		}
	}

	public static void NotifyItemCraft(Recipe recipe)
	{
		if (OnItemCraft != null)
		{
			OnItemCraft((short)recipe.createItem.type, recipe.createItem.stack);
		}
	}

	public static void TryGrantingBestiary100PercentAchievement()
	{
		if (Main.GetBestiaryProgressReport().CompletionPercent >= 1f)
		{
			NotifyProgressionEvent(29);
		}
	}

	public static void Initialize()
	{
		Player.Hooks.OnEnterWorld += OnPlayerEnteredWorld;
	}

	internal static void OnPlayerEnteredWorld(Player player)
	{
		if (OnItemPickup != null)
		{
			for (int i = 0; i < 58; i++)
			{
				OnItemPickup(player, (short)player.inventory[i].type, player.inventory[i].stack);
			}
			for (int j = 0; j < player.armor.Length; j++)
			{
				OnItemPickup(player, (short)player.armor[j].type, player.armor[j].stack);
			}
			for (int k = 0; k < player.dye.Length; k++)
			{
				OnItemPickup(player, (short)player.dye[k].type, player.dye[k].stack);
			}
			for (int l = 0; l < player.miscEquips.Length; l++)
			{
				OnItemPickup(player, (short)player.miscEquips[l].type, player.miscEquips[l].stack);
			}
			for (int m = 0; m < player.miscDyes.Length; m++)
			{
				OnItemPickup(player, (short)player.miscDyes[m].type, player.miscDyes[m].stack);
			}
			for (int n = 0; n < player.bank.item.Length; n++)
			{
				OnItemPickup(player, (short)player.bank.item[n].type, player.bank.item[n].stack);
			}
			for (int num = 0; num < player.bank2.item.Length; num++)
			{
				OnItemPickup(player, (short)player.bank2.item[num].type, player.bank2.item[num].stack);
			}
			for (int num2 = 0; num2 < player.bank3.item.Length; num2++)
			{
				OnItemPickup(player, (short)player.bank3.item[num2].type, player.bank3.item[num2].stack);
			}
			for (int num3 = 0; num3 < player.bank4.item.Length; num3++)
			{
				OnItemPickup(player, (short)player.bank4.item[num3].type, player.bank4.item[num3].stack);
			}
			for (int num4 = 0; num4 < player.Loadouts.Length; num4++)
			{
				Item[] armor = player.Loadouts[num4].Armor;
				for (int num5 = 0; num5 < armor.Length; num5++)
				{
					OnItemPickup(player, (short)armor[num5].type, armor[num5].stack);
				}
				armor = player.Loadouts[num4].Dye;
				for (int num6 = 0; num6 < armor.Length; num6++)
				{
					OnItemPickup(player, (short)armor[num6].type, armor[num6].stack);
				}
			}
		}
		if (player.statManaMax > 20)
		{
			Main.Achievements.GetCondition("STAR_POWER", "Use").Complete();
		}
		if (player.statLifeMax == 500 && player.statManaMax == 200)
		{
			Main.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
		}
		if (player.miscEquips[4].type > 0)
		{
			Main.Achievements.GetCondition("HOLD_ON_TIGHT", "Equip").Complete();
		}
		if (player.miscEquips[3].type > 0)
		{
			Main.Achievements.GetCondition("THE_CAVALRY", "Equip").Complete();
		}
		for (int num7 = 0; num7 < player.armor.Length; num7++)
		{
			if (player.armor[num7].wingSlot > 0)
			{
				Main.Achievements.GetCondition("HEAD_IN_THE_CLOUDS", "Equip").Complete();
				break;
			}
		}
		if (player.armor[0].stack > 0 && player.armor[1].stack > 0 && player.armor[2].stack > 0)
		{
			Main.Achievements.GetCondition("MATCHING_ATTIRE", "Equip").Complete();
		}
		if (player.armor[10].stack > 0 && player.armor[11].stack > 0 && player.armor[12].stack > 0)
		{
			Main.Achievements.GetCondition("FASHION_STATEMENT", "Equip").Complete();
		}
		bool flag = true;
		for (int num8 = 0; num8 < 10; num8++)
		{
			if (player.IsItemSlotUnlockedAndUsable(num8) && (player.dye[num8].type < 1 || player.dye[num8].stack < 1))
			{
				flag = false;
			}
		}
		if (flag)
		{
			Main.Achievements.GetCondition("DYE_HARD", "Equip").Complete();
		}
		if (player.unlockedBiomeTorches)
		{
			Main.Achievements.GetCondition("GAIN_TORCH_GODS_FAVOR", "Use").Complete();
		}
		WorldGen.CheckAchievement_RealEstateAndTownSlimes();
		TryGrantingBestiary100PercentAchievement();
	}

	public static void NotifyNPCKilled(NPC npc)
	{
		if (Main.netMode == 0)
		{
			if (npc.playerInteraction[Main.myPlayer])
			{
				NotifyNPCKilledDirect(Main.player[Main.myPlayer], npc.netID);
			}
			return;
		}
		for (int i = 0; i < 255; i++)
		{
			if (npc.playerInteraction[i])
			{
				NetMessage.SendData(97, i, -1, null, npc.netID);
			}
		}
	}

	public static void NotifyNPCKilledDirect(Player player, int npcNetID)
	{
		if (OnNPCKilled != null)
		{
			OnNPCKilled(player, (short)npcNetID);
		}
	}

	public static void NotifyProgressionEvent(int eventID)
	{
		if (Main.netMode == 2)
		{
			NetMessage.SendData(98, -1, -1, null, eventID);
		}
		else if (OnProgressionEvent != null)
		{
			OnProgressionEvent(eventID);
		}
	}

	public static void HandleOnEquip(Player player, Item item, int context)
	{
		if (context == 16)
		{
			Main.Achievements.GetCondition("HOLD_ON_TIGHT", "Equip").Complete();
		}
		if (context == 17)
		{
			Main.Achievements.GetCondition("THE_CAVALRY", "Equip").Complete();
		}
		if ((context == 10 || context == 11) && item.wingSlot > 0)
		{
			Main.Achievements.GetCondition("HEAD_IN_THE_CLOUDS", "Equip").Complete();
		}
		if (context == 8 && player.armor[0].stack > 0 && player.armor[1].stack > 0 && player.armor[2].stack > 0)
		{
			Main.Achievements.GetCondition("MATCHING_ATTIRE", "Equip").Complete();
		}
		if (context == 9 && player.armor[10].stack > 0 && player.armor[11].stack > 0 && player.armor[12].stack > 0)
		{
			Main.Achievements.GetCondition("FASHION_STATEMENT", "Equip").Complete();
		}
		if (context != 12 && context != 33)
		{
			return;
		}
		for (int i = 0; i < 10; i++)
		{
			if (player.IsItemSlotUnlockedAndUsable(i) && (player.dye[i].type < 1 || player.dye[i].stack < 1))
			{
				return;
			}
		}
		for (int j = 0; j < player.miscDyes.Length; j++)
		{
			if (player.miscDyes[j].type < 1 || player.miscDyes[j].stack < 1)
			{
				return;
			}
		}
		Main.Achievements.GetCondition("DYE_HARD", "Equip").Complete();
	}

	public static void HandleSpecialEvent(Player player, int eventID)
	{
		if (player.whoAmI != Main.myPlayer)
		{
			return;
		}
		switch (eventID)
		{
		case 1:
			Main.Achievements.GetCondition("STAR_POWER", "Use").Complete();
			if (player.statLifeMax == 500 && player.statManaMax == 200)
			{
				Main.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
			}
			break;
		case 2:
			Main.Achievements.GetCondition("GET_A_LIFE", "Use").Complete();
			if (player.statLifeMax == 500 && player.statManaMax == 200)
			{
				Main.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
			}
			break;
		case 3:
			Main.Achievements.GetCondition("NOT_THE_BEES", "Use").Complete();
			break;
		case 4:
			Main.Achievements.GetCondition("WATCH_YOUR_STEP", "Hit").Complete();
			break;
		case 6:
			Main.Achievements.GetCondition("YOU_AND_WHAT_ARMY", "Spawn").Complete();
			break;
		case 5:
			Main.Achievements.GetCondition("RAINBOWS_AND_UNICORNS", "Use").Complete();
			break;
		case 7:
			Main.Achievements.GetCondition("THROWING_LINES", "Use").Complete();
			break;
		case 17:
			Main.Achievements.GetCondition("FLY_A_KITE_ON_A_WINDY_DAY", "Use").Complete();
			break;
		case 8:
			Main.Achievements.GetCondition("LUCKY_BREAK", "Hit").Complete();
			break;
		case 9:
			Main.Achievements.GetCondition("VEHICULAR_MANSLAUGHTER", "Hit").Complete();
			break;
		case 10:
			Main.Achievements.GetCondition("ROCK_BOTTOM", "Reach").Complete();
			break;
		case 11:
			Main.Achievements.GetCondition("INTO_ORBIT", "Reach").Complete();
			break;
		case 12:
			Main.Achievements.GetCondition("WHERES_MY_HONEY", "Reach").Complete();
			break;
		case 13:
			Main.Achievements.GetCondition("JEEPERS_CREEPERS", "Reach").Complete();
			break;
		case 14:
			Main.Achievements.GetCondition("ITS_GETTING_HOT_IN_HERE", "Reach").Complete();
			break;
		case 15:
			Main.Achievements.GetCondition("FUNKYTOWN", "Reach").Complete();
			break;
		case 16:
			Main.Achievements.GetCondition("I_AM_LOOT", "Peek").Complete();
			break;
		case 18:
			Main.Achievements.GetCondition("FOUND_GRAVEYARD", "Reach").Complete();
			break;
		case 19:
			Main.Achievements.GetCondition("GO_LAVA_FISHING", "Do").Complete();
			break;
		case 20:
			Main.Achievements.GetCondition("TALK_TO_NPC_AT_MAX_HAPPINESS", "Do").Complete();
			break;
		case 21:
			Main.Achievements.GetCondition("PET_THE_PET", "Do").Complete();
			break;
		case 22:
			Main.Achievements.GetCondition("FIND_A_FAIRY", "Do").Complete();
			break;
		case 23:
			Main.Achievements.GetCondition("DIE_TO_DEAD_MANS_CHEST", "Do").Complete();
			break;
		case 24:
			Main.Achievements.GetCondition("GAIN_TORCH_GODS_FAVOR", "Use").Complete();
			break;
		case 25:
			Main.Achievements.GetCondition("DRINK_BOTTLED_WATER_WHILE_DROWNING", "Use").Complete();
			break;
		case 26:
			Main.Achievements.GetCondition("PLAY_ON_A_SPECIAL_SEED", "Do").Complete();
			break;
		case 27:
			Main.Achievements.GetCondition("PURIFY_ENTIRE_WORLD", "Do").Complete();
			break;
		}
	}

	public static void DoClassicTitleScreenAchievement()
	{
		Main.Achievements.GetCondition("GOING_OLDSCHOOL", "Do").Complete();
	}

	public static void CheckResearchAchievement(bool forced = false)
	{
		int lastEditId = Main.LocalPlayerCreativeTracker.ItemSacrifices.LastEditId;
		if (forced || _lastResearchVersion != lastEditId)
		{
			_lastResearchVersion = lastEditId;
			Main.LocalPlayerCreativeTracker.ItemSacrifices.CountFullyResearchedItems(out var fullyResearchedItems, out var allItems);
			if (fullyResearchedItems >= allItems / 2)
			{
				NotifyProgressionEvent(45);
			}
		}
	}

	public static void PlantedAcorn()
	{
		((CustomIntCondition)Main.Achievements.GetCondition("CONSERVATIONIST", "Do")).Value++;
	}

	public static void HandleNurseService(int coinsSpent)
	{
		((CustomFloatCondition)Main.Achievements.GetCondition("FREQUENT_FLYER", "Pay")).Value += coinsSpent;
	}

	public static void HandleAnglerService()
	{
		Main.Achievements.GetCondition("SERVANT_IN_TRAINING", "Finish").Complete();
		((CustomIntCondition)Main.Achievements.GetCondition("GOOD_LITTLE_SLAVE", "Finish")).Value++;
		((CustomIntCondition)Main.Achievements.GetCondition("TROUT_MONKEY", "Finish")).Value++;
		((CustomIntCondition)Main.Achievements.GetCondition("FAST_AND_FISHIOUS", "Finish")).Value++;
		((CustomIntCondition)Main.Achievements.GetCondition("SUPREME_HELPER_MINION", "Finish")).Value++;
	}

	public static void HandleRunning(float pixelsMoved)
	{
		((CustomFloatCondition)Main.Achievements.GetCondition("MARATHON_MEDALIST", "Move")).Value += pixelsMoved;
	}

	public static void HandleMining()
	{
		((CustomIntCondition)Main.Achievements.GetCondition("BULLDOZER", "Pick")).Value++;
	}

	public static void MechaMayhem_Clear()
	{
		ScanForMechs(out var foundSkeletronPrime, out var foundDestroyer, out var foundTwins);
		if (!(foundSkeletronPrime | foundDestroyer | foundTwins))
		{
			mayhem1down = false;
			mayhem2down = false;
			mayhem3down = false;
		}
	}

	public static void MechaMayhem_Start()
	{
		ScanForMechs(out var foundSkeletronPrime, out var foundDestroyer, out var foundTwins);
		mayhemOK = foundSkeletronPrime & foundDestroyer & foundTwins;
	}

	public static void MechaMayhem_Kill(int justKilled)
	{
		if (mayhemOK)
		{
			if (justKilled == 125 || justKilled == 126)
			{
				mayhem1down = true;
			}
			else if (!NPC.AnyNPCs(125) && !NPC.AnyNPCs(126) && !mayhem1down)
			{
				mayhemOK = false;
				return;
			}
			if (justKilled == 134)
			{
				mayhem2down = true;
			}
			else if (!NPC.AnyNPCs(134) && !mayhem2down)
			{
				mayhemOK = false;
				return;
			}
			if (justKilled == 127)
			{
				mayhem3down = true;
			}
			else if (!NPC.AnyNPCs(127) && !mayhem3down)
			{
				mayhemOK = false;
				return;
			}
			if (mayhem1down && mayhem2down && mayhem3down)
			{
				NotifyProgressionEvent(21);
			}
		}
	}

	private static void ScanForMechs(out bool foundSkeletronPrime, out bool foundDestroyer, out bool foundTwins)
	{
		foundSkeletronPrime = false;
		foundDestroyer = false;
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active)
			{
				int type = nPC.type;
				foundSkeletronPrime |= type == 127;
				foundDestroyer |= type == 134;
				flag |= type == 126;
				flag2 |= type == 125;
			}
		}
		foundTwins = flag & flag2;
	}
}
