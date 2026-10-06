using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria;

public class Lang
{
	private class ItemPrefixCombiner
	{
		public string ItemName { get; set; }

		public string PrefixName { get; set; }
	}

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] menu = new LocalizedText[254];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] gen = new LocalizedText[94];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] misc = new LocalizedText[201];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] inter = new LocalizedText[129];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] tip = new LocalizedText[62];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] mp = new LocalizedText[27];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] chestType = new LocalizedText[54];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] dresserType = new LocalizedText[65];

	[Old("Lang arrays have been replaced with the new Language.GetText system.")]
	public static LocalizedText[] chestType2 = new LocalizedText[38];

	public static LocalizedText[] prefix = new LocalizedText[PrefixID.Count];

	public static LocalizedText[] _mapLegendCache;

	private static LocalizedText[] _itemNameCache = new LocalizedText[ItemID.Count];

	private static LocalizedText[] _projectileNameCache = new LocalizedText[ProjectileID.Count];

	private static LocalizedText[] _npcNameCache = new LocalizedText[NPCID.Count];

	private static LocalizedText[] _negativeNpcNameCache = new LocalizedText[65];

	private static LocalizedText[] _buffNameCache = new LocalizedText[BuffID.Count];

	private static LocalizedText[] _buffDescriptionCache = new LocalizedText[BuffID.Count];

	private static ItemTooltip[] _itemTooltipCache = new ItemTooltip[ItemID.Count];

	private static LocalizedText[] _emojiNameCache = new LocalizedText[EmoteID.Count];

	private static Dictionary<string, Func<object>> _globalSubstitutions = new Dictionary<string, Func<object>>();

	private static LocalizedText _prefixFormatText = Language.GetText("CombineFormat.Prefix");

	public static string GetMapObjectName(int id)
	{
		if (_mapLegendCache != null)
		{
			return _mapLegendCache[id].Value;
		}
		return string.Empty;
	}

	public static void RegisterGlobalSubstitution(string key, Func<object> getValue)
	{
		_globalSubstitutions[key] = getValue;
	}

	public static object GetGlobalSubstitution(string key)
	{
		if (!_globalSubstitutions.TryGetValue(key, out var value))
		{
			return null;
		}
		return value();
	}

	private static void InitGlobalSubstitutions()
	{
		RegisterGlobalSubstitution("Nurse", () => NPC.GetFirstNPCNameOrNull(18));
		RegisterGlobalSubstitution("Merchant", () => NPC.GetFirstNPCNameOrNull(17));
		RegisterGlobalSubstitution("ArmsDealer", () => NPC.GetFirstNPCNameOrNull(19));
		RegisterGlobalSubstitution("Dryad", () => NPC.GetFirstNPCNameOrNull(20));
		RegisterGlobalSubstitution("Demolitionist", () => NPC.GetFirstNPCNameOrNull(38));
		RegisterGlobalSubstitution("Clothier", () => NPC.GetFirstNPCNameOrNull(54));
		RegisterGlobalSubstitution("Guide", () => NPC.GetFirstNPCNameOrNull(22));
		RegisterGlobalSubstitution("Wizard", () => NPC.GetFirstNPCNameOrNull(108));
		RegisterGlobalSubstitution("GoblinTinkerer", () => NPC.GetFirstNPCNameOrNull(107));
		RegisterGlobalSubstitution("Mechanic", () => NPC.GetFirstNPCNameOrNull(124));
		RegisterGlobalSubstitution("Truffle", () => NPC.GetFirstNPCNameOrNull(160));
		RegisterGlobalSubstitution("Steampunker", () => NPC.GetFirstNPCNameOrNull(178));
		RegisterGlobalSubstitution("DyeTrader", () => NPC.GetFirstNPCNameOrNull(207));
		RegisterGlobalSubstitution("PartyGirl", () => NPC.GetFirstNPCNameOrNull(208));
		RegisterGlobalSubstitution("Cyborg", () => NPC.GetFirstNPCNameOrNull(209));
		RegisterGlobalSubstitution("Painter", () => NPC.GetFirstNPCNameOrNull(227));
		RegisterGlobalSubstitution("WitchDoctor", () => NPC.GetFirstNPCNameOrNull(228));
		RegisterGlobalSubstitution("Pirate", () => NPC.GetFirstNPCNameOrNull(229));
		RegisterGlobalSubstitution("Stylist", () => NPC.GetFirstNPCNameOrNull(353));
		RegisterGlobalSubstitution("TravelingMerchant", () => NPC.GetFirstNPCNameOrNull(368));
		RegisterGlobalSubstitution("Angler", () => NPC.GetFirstNPCNameOrNull(369));
		RegisterGlobalSubstitution("Bartender", () => NPC.GetFirstNPCNameOrNull(550));
		RegisterGlobalSubstitution("WorldName", () => Main.worldName);
		RegisterGlobalSubstitution("Day", () => Main.dayTime);
		RegisterGlobalSubstitution("BloodMoon", () => Main.bloodMoon);
		RegisterGlobalSubstitution("Eclipse", () => Main.eclipse);
		RegisterGlobalSubstitution("MoonLordDefeated", () => NPC.downedMoonlord);
		RegisterGlobalSubstitution("GolemDefeated", () => NPC.downedGolemBoss);
		RegisterGlobalSubstitution("DukeFishronDefeated", () => NPC.downedFishron);
		RegisterGlobalSubstitution("FrostLegionDefeated", () => NPC.downedFrost);
		RegisterGlobalSubstitution("MartiansDefeated", () => NPC.downedMartians);
		RegisterGlobalSubstitution("PumpkingDefeated", () => NPC.downedHalloweenKing);
		RegisterGlobalSubstitution("IceQueenDefeated", () => NPC.downedChristmasIceQueen);
		RegisterGlobalSubstitution("HardMode", () => Main.hardMode);
		RegisterGlobalSubstitution("Homeless", () => Main.LocalPlayer.talkNPC >= 0 && Main.npc[Main.LocalPlayer.talkNPC].homeless);
		RegisterGlobalSubstitution("InventoryKey", () => Main.cInv);
		RegisterGlobalSubstitution("PlayerName", () => Main.player[Main.myPlayer].name);
		RegisterGlobalSubstitution("GolfGuy", () => NPC.GetFirstNPCNameOrNull(588));
		RegisterGlobalSubstitution("TaxCollector", () => NPC.GetFirstNPCNameOrNull(441));
		RegisterGlobalSubstitution("Rain", () => Main.raining);
		RegisterGlobalSubstitution("Graveyard", () => Main.LocalPlayer.ZoneGraveyard);
		RegisterGlobalSubstitution("AnglerCompletedQuestsCount", () => Main.LocalPlayer.anglerQuestsFinished);
		RegisterGlobalSubstitution("TotalDeathsCount", () => Main.LocalPlayer.numberOfDeathsPVE);
		RegisterGlobalSubstitution("WorldEvilStone", () => (!WorldGen.crimson) ? Language.GetTextValue("Misc.Ebonstone") : Language.GetTextValue("Misc.Crimstone"));
		RegisterGlobalSubstitution("InputTriggerUI_BuildFromInventory", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "QuickMount"));
		RegisterGlobalSubstitution("InputTriggerUI_TrashEnabled", () => !ItemSlot.Options.DisableQuickTrash);
		RegisterGlobalSubstitution("InputTriggerUI_Trash", () => (!PlayerInput.UsingGamepad) ? Language.GetTextValue(ItemSlot.Options.DisableLeftShiftTrashCan ? "Controls.Control" : "Controls.Shift") : PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "SmartSelect"));
		RegisterGlobalSubstitution("InputTriggerUI_FavoriteItem", () => (!PlayerInput.UsingGamepad) ? ((object)Main.FavoriteKey/*cast due to constrained. prefix*/).ToString() : PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "SmartCursor"));
		RegisterGlobalSubstitution("InputTrigger_QuickEquip", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "Grapple"));
		RegisterGlobalSubstitution("InputTrigger_Grapple", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "Grapple"));
		RegisterGlobalSubstitution("InputTrigger_LockOn", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "LockOn"));
		RegisterGlobalSubstitution("InputTrigger_RadialQuickbar", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "HotbarMinus"));
		RegisterGlobalSubstitution("InputTrigger_RadialHotbar", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "HotbarPlus"));
		RegisterGlobalSubstitution("InputTrigger_SmartCursor", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "SmartCursor"));
		RegisterGlobalSubstitution("InputTrigger_UseOrAttack", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "MouseLeft"));
		RegisterGlobalSubstitution("InputTrigger_Jump", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "Jump"));
		RegisterGlobalSubstitution("InputTrigger_InteractWithTile", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "MouseRight"));
		RegisterGlobalSubstitution("InputTrigger_InteractWithTileUI", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "MouseRight"));
		RegisterGlobalSubstitution("InputTrigger_ToggleOrOpen", () => (!PlayerInput.UsingGamepad) ? PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "MouseRight") : PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "Grapple"));
		RegisterGlobalSubstitution("InputTrigger_SmartSelect", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "SmartSelect"));
		RegisterGlobalSubstitution("InputTrigger_Mount", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "QuickMount"));
		RegisterGlobalSubstitution("InputTrigger_Healing", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "QuickHeal"));
		RegisterGlobalSubstitution("InputTrigger_ManaHealing", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "QuickMana"));
		RegisterGlobalSubstitution("InputTrigger_QuickBuff", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: true, "QuickBuff"));
		RegisterGlobalSubstitution("ToggleArmorSetBonusKey", () => Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN"));
	}

	[Old("dialog is deprecated. Please use Language.GetText instead.")]
	public static string dialog(int l, bool english = false)
	{
		return Language.GetTextValue("LegacyDialog." + l);
	}

	public static string GetNPCNameValue(int netID)
	{
		return GetNPCName(netID).Value;
	}

	public static LocalizedText GetNPCName(int netID)
	{
		if (netID > 0 && netID < NPCID.Count)
		{
			return _npcNameCache[netID];
		}
		if (netID < 0 && -netID - 1 < _negativeNpcNameCache.Length)
		{
			return _negativeNpcNameCache[-netID - 1];
		}
		return LocalizedText.Empty;
	}

	public static ItemTooltip GetTooltip(int itemId)
	{
		return _itemTooltipCache[itemId];
	}

	public static LocalizedText GetItemName(int id)
	{
		if (id < 0)
		{
			id = ItemID.FromNetId((short)id);
		}
		if (id > 0 && id < ItemID.Count && _itemNameCache[id] != null)
		{
			return _itemNameCache[id];
		}
		return LocalizedText.Empty;
	}

	public static LocalizedText GetEmojiName(int id)
	{
		if (id >= 0 && id < EmoteID.Count && _emojiNameCache[id] != null)
		{
			return _emojiNameCache[id];
		}
		return LocalizedText.Empty;
	}

	public static string GetItemNameValue(int id)
	{
		return GetItemName(id).Value;
	}

	public static string GetPrefixedItemName(int id, int prefixType)
	{
		LocalizedText itemName = GetItemName(id);
		LocalizedText localizedText = prefix[prefixType];
		string prefixName = localizedText.Value;
		if (Language.TryGetVariation(itemName.Key, "Gender", out var value) && Language.TryGetVariation(localizedText.Key, value, out var value2))
		{
			prefixName = value2;
		}
		return _prefixFormatText.FormatWith(new ItemPrefixCombiner
		{
			PrefixName = prefixName,
			ItemName = itemName.Value
		});
	}

	public static string GetBuffName(int id)
	{
		return _buffNameCache[id].Value;
	}

	public static string GetBuffDescription(int id)
	{
		return _buffDescriptionCache[id].Value;
	}

	public static string GetDryadWorldStatusDialog(out bool worldIsEntirelyPure)
	{
		string text = "";
		worldIsEntirelyPure = false;
		int tGood = WorldGen.tGood;
		int tEvil = WorldGen.tEvil;
		int tBlood = WorldGen.tBlood;
		if (tGood > 0 && tEvil > 0 && tBlood > 0)
		{
			text = Language.GetTextValue("DryadSpecialText.WorldStatusAll", Main.worldName, tGood, tEvil, tBlood);
		}
		else if (tGood > 0 && tEvil > 0)
		{
			text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCorrupt", Main.worldName, tGood, tEvil);
		}
		else if (tGood > 0 && tBlood > 0)
		{
			text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCrimson", Main.worldName, tGood, tBlood);
		}
		else if (tEvil > 0 && tBlood > 0)
		{
			text = Language.GetTextValue("DryadSpecialText.WorldStatusCorruptCrimson", Main.worldName, tEvil, tBlood);
		}
		else if (tEvil > 0)
		{
			text = Language.GetTextValue("DryadSpecialText.WorldStatusCorrupt", Main.worldName, tEvil);
		}
		else if (tBlood > 0)
		{
			text = Language.GetTextValue("DryadSpecialText.WorldStatusCrimson", Main.worldName, tBlood);
		}
		else
		{
			if (tGood <= 0)
			{
				text = Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
				worldIsEntirelyPure = true;
				return text;
			}
			text = Language.GetTextValue("DryadSpecialText.WorldStatusHallow", Main.worldName, tGood);
		}
		string arg;
		if ((double)tGood * 1.2 >= (double)(tEvil + tBlood) && (double)tGood * 0.8 <= (double)(tEvil + tBlood))
		{
			arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced");
		}
		else if (tGood >= tEvil + tBlood)
		{
			arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale");
		}
		else if (tEvil + tBlood > tGood + 20)
		{
			arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim");
		}
		else
		{
			arg = ((tEvil + tBlood <= 5) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionClose") : Language.GetTextValue("DryadSpecialText.WorldDescriptionWork"));
		}
		return $"{text} {arg}";
	}

	public static string GetRandomGameTitle()
	{
		return Language.RandomFromCategory("GameTitle").Value;
	}

	public static string DyeTraderQuestChat(bool gotDye = false)
	{
		LocalizedText[] array = Language.FindAll(CreateDialogFilter(gotDye ? "DyeTraderSpecialText.HasPlant" : "DyeTraderSpecialText.NoPlant"));
		return array[Main.rand.Next(array.Length)].Value;
	}

	public static string AnglerQuestCountChat()
	{
		return Language.SelectRandom(CreateDialogFilter("AnglerQuestChatter.")).Value;
	}

	public static string BartenderHelpText(NPC npc)
	{
		Player player = Main.player[Main.myPlayer];
		if (player.bartenderQuestLog == 0)
		{
			player.bartenderQuestLog++;
			Item item = new Item();
			item.SetDefaults(3817);
			item.stack = 10;
			player.QuickSpawnItem(new EntitySource_Gift(npc), item, GetItemSettings.GiftRecieved);
			return Language.GetTextValue("BartenderSpecialText.FirstHelp");
		}
		LocalizedText[] array = Language.FindAll(CreateDialogFilter("BartenderHelpText."));
		if (Main.BartenderHelpTextIndex >= array.Length)
		{
			Main.BartenderHelpTextIndex = 0;
		}
		return array[Main.BartenderHelpTextIndex++].Value;
	}

	public static string BartenderChat()
	{
		if (Main.rand.Next(5) == 0)
		{
			string key;
			if (DD2Event.DownedInvasionT3)
			{
				key = "BartenderSpecialText.AfterDD2Tier3";
			}
			else if (DD2Event.DownedInvasionT2)
			{
				key = "BartenderSpecialText.AfterDD2Tier2";
			}
			else
			{
				key = ((!DD2Event.DownedInvasionT1) ? "BartenderSpecialText.BeforeDD2Tier1" : "BartenderSpecialText.AfterDD2Tier1");
			}
			return Language.GetTextValue(key);
		}
		return Language.SelectRandom(CreateDialogFilter("BartenderChatter.")).Value;
	}

	public static string GolferChat()
	{
		return Language.SelectRandom(CreateDialogFilter("GolferChatter.")).Value;
	}

	public static string BestiaryGirlChat()
	{
		string startsWith = "BestiaryGirlChatter.";
		if (NPC.ShouldBestiaryGirlBeLycantrope())
		{
			startsWith = "BestiaryGirlLycantropeChatter.";
		}
		return Language.SelectRandom(CreateDialogFilter(startsWith)).Value;
	}

	public static string PrincessChat()
	{
		return Language.SelectRandom(CreateDialogFilter("PrincessChatter.")).Value;
	}

	public static string CatChat()
	{
		return Language.SelectRandom(CreateDialogFilter("CatChatter.Chatter")).Value;
	}

	public static string DogChat()
	{
		return Language.SelectRandom(CreateDialogFilter("DogChatter.Chatter")).Value;
	}

	public static string BunnyChat()
	{
		return Language.SelectRandom(CreateDialogFilter("BunnyChatter.Chatter")).Value;
	}

	public static string GetSlimeType(NPC npc)
	{
		string result = "Blue";
		switch (npc.type)
		{
		case 670:
			result = "Blue";
			break;
		case 678:
			result = "Green";
			break;
		case 679:
			result = "Old";
			break;
		case 681:
			result = "Rainbow";
			break;
		case 680:
			result = "Purple";
			break;
		case 682:
			result = "Red";
			break;
		case 683:
			result = "Yellow";
			break;
		case 684:
			result = "Copper";
			break;
		}
		return result;
	}

	public static string SlimeChat(NPC npc)
	{
		string slimeType = GetSlimeType(npc);
		return Language.SelectRandom(CreateDialogFilter("Slime" + slimeType + "Chatter.Chatter")).Value;
	}

	public static string GetNPCHouseBannerText(NPC npc, int bannerStyle)
	{
		if (bannerStyle == 1)
		{
			return Language.GetTextValue("Game.ReservedForNPC", npc.FullName);
		}
		return npc.FullName;
	}

	public static LanguageSearchFilter CreateDialogFilter(string startsWith, bool checkConditions = true)
	{
		return (string key, LocalizedText text) => key.StartsWith(startsWith) && (!checkConditions || text.ConditionsMet);
	}

	public static string AnglerQuestChat(bool turnIn = false)
	{
		if (turnIn)
		{
			return Language.SelectRandom(CreateDialogFilter("AnglerQuestText.TurnIn_")).Value;
		}
		if (Main.anglerQuestFinished)
		{
			return Language.SelectRandom(CreateDialogFilter("AnglerQuestText.NoQuest_")).Value;
		}
		int num = (Main.npcChatCornerItem = Main.anglerQuestItemNetIDs[Main.anglerQuest]);
		return Language.GetTextValue("AnglerQuestText.Quest_" + ItemID.Search.GetName(num));
	}

	public static LocalizedText GetProjectileName(int type)
	{
		if (type >= 0 && type < _projectileNameCache.Length && _projectileNameCache[type] != null)
		{
			return _projectileNameCache[type];
		}
		return LocalizedText.Empty;
	}

	private static void FillNameCacheArray<IdClass, IdType>(string category, LocalizedText[] nameCache, bool leaveMissingEntriesBlank = false) where IdType : IConvertible
	{
		for (int i = 0; i < nameCache.Length; i++)
		{
			nameCache[i] = LocalizedText.Empty;
		}
		(from f in typeof(IdClass).GetFields(BindingFlags.Static | BindingFlags.Public)
			where f.FieldType == typeof(IdType)
			select f).ToList().ForEach((FieldInfo field) =>
		{
			long num = Convert.ToInt64((IdType)field.GetValue(null));
			if (num >= 0 && num < nameCache.Length)
			{
				nameCache[num] = ((!leaveMissingEntriesBlank || Language.Exists(category + "." + field.Name)) ? Language.GetText(category + "." + field.Name) : LocalizedText.Empty);
			}
			else if (field.Name == "None")
			{
				nameCache[num] = LocalizedText.Empty;
			}
		});
	}

	public static void InitializeLegacyLocalization()
	{
		FillNameCacheArray<PrefixID, int>("Prefix", prefix);
		for (int i = 0; i < gen.Length; i++)
		{
			gen[i] = Language.GetText("LegacyWorldGen." + i);
		}
		for (int j = 0; j < menu.Length; j++)
		{
			menu[j] = Language.GetText("LegacyMenu." + j);
		}
		for (int k = 0; k < inter.Length; k++)
		{
			inter[k] = Language.GetText("LegacyInterface." + k);
		}
		for (int l = 0; l < misc.Length; l++)
		{
			misc[l] = Language.GetText("LegacyMisc." + l);
		}
		for (int m = 0; m < mp.Length; m++)
		{
			mp[m] = Language.GetText("LegacyMultiplayer." + m);
		}
		for (int n = 0; n < tip.Length; n++)
		{
			tip[n] = Language.GetText("LegacyTooltip." + n);
		}
		for (int num = 0; num < 52; num++)
		{
			chestType[num] = Language.GetText("LegacyChestType." + num);
		}
		for (int num2 = 52; num2 < chestType.Length; num2++)
		{
			chestType[num2] = chestType[0];
		}
		for (int num3 = 0; num3 < chestType2.Length; num3++)
		{
			chestType2[num3] = Language.GetText("LegacyChestType2." + num3);
		}
		for (int num4 = 0; num4 < dresserType.Length; num4++)
		{
			dresserType[num4] = Language.GetText("LegacyDresserType." + num4);
		}
		FillNameCacheArray<ItemID, short>("ItemName", _itemNameCache);
		FillNameCacheArray<ProjectileID, short>("ProjectileName", _projectileNameCache);
		FillNameCacheArray<NPCID, short>("NPCName", _npcNameCache);
		FillNameCacheArray<BuffID, int>("BuffName", _buffNameCache);
		FillNameCacheArray<BuffID, int>("BuffDescription", _buffDescriptionCache);
		FillNameCacheArray<EmoteID, int>("EmojiName", _emojiNameCache, leaveMissingEntriesBlank: true);
		for (int num5 = -65; num5 < 0; num5++)
		{
			_negativeNpcNameCache[-num5 - 1] = _npcNameCache[NPCID.FromNetId(num5)];
		}
		_negativeNpcNameCache[0] = Language.GetText("NPCName.Slimeling");
		_negativeNpcNameCache[1] = Language.GetText("NPCName.Slimer2");
		_negativeNpcNameCache[2] = Language.GetText("NPCName.GreenSlime");
		_negativeNpcNameCache[3] = Language.GetText("NPCName.Pinky");
		_negativeNpcNameCache[4] = Language.GetText("NPCName.BabySlime");
		_negativeNpcNameCache[5] = Language.GetText("NPCName.BlackSlime");
		_negativeNpcNameCache[6] = Language.GetText("NPCName.PurpleSlime");
		_negativeNpcNameCache[7] = Language.GetText("NPCName.RedSlime");
		_negativeNpcNameCache[8] = Language.GetText("NPCName.YellowSlime");
		_negativeNpcNameCache[9] = Language.GetText("NPCName.JungleSlime");
		_negativeNpcNameCache[53] = Language.GetText("NPCName.SmallRainZombie");
		_negativeNpcNameCache[54] = Language.GetText("NPCName.BigRainZombie");
		for (int num6 = 0; num6 < _itemTooltipCache.Length; num6++)
		{
			_itemTooltipCache[num6] = ItemTooltip.None;
		}
		(from f in typeof(ItemID).GetFields(BindingFlags.Static | BindingFlags.Public)
			where f.FieldType == typeof(short)
			select f).ToList().ForEach((FieldInfo field) =>
		{
			short num7 = (short)field.GetValue(null);
			if (num7 > 0 && num7 < _itemTooltipCache.Length)
			{
				_itemTooltipCache[num7] = ItemTooltip.FromLanguageKey("ItemTooltip." + field.Name);
			}
		});
		InitGlobalSubstitutions();
	}

	public static void BuildMapAtlas()
	{
		if (Main.dedServ)
		{
			return;
		}
		_mapLegendCache = new LocalizedText[MapHelper.LookupCount()];
		for (int i = 0; i < _mapLegendCache.Length; i++)
		{
			_mapLegendCache[i] = LocalizedText.Empty;
		}
		for (int j = 0; j < TileID.Sets.IsATreeTrunk.Length; j++)
		{
			if (TileID.Sets.IsATreeTrunk[j])
			{
				_mapLegendCache[MapHelper.TileToLookup(j, 0)] = Language.GetText("MapObject.Tree");
			}
		}
		LocalizedText[] itemNameCache = _itemNameCache;
		_mapLegendCache[MapHelper.TileToLookup(4, 0)] = itemNameCache[8];
		_mapLegendCache[MapHelper.TileToLookup(4, 1)] = itemNameCache[8];
		_mapLegendCache[MapHelper.TileToLookup(6, 0)] = Language.GetText("MapObject.Iron");
		_mapLegendCache[MapHelper.TileToLookup(7, 0)] = Language.GetText("MapObject.Copper");
		_mapLegendCache[MapHelper.TileToLookup(8, 0)] = Language.GetText("MapObject.Gold");
		_mapLegendCache[MapHelper.TileToLookup(9, 0)] = Language.GetText("MapObject.Silver");
		_mapLegendCache[MapHelper.TileToLookup(10, 0)] = Language.GetText("MapObject.Door");
		_mapLegendCache[MapHelper.TileToLookup(11, 0)] = Language.GetText("MapObject.Door");
		_mapLegendCache[MapHelper.TileToLookup(12, 0)] = itemNameCache[29];
		_mapLegendCache[MapHelper.TileToLookup(665, 0)] = itemNameCache[29];
		_mapLegendCache[MapHelper.TileToLookup(711, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(664, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(712, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(713, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(714, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(715, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(716, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(639, 0)] = itemNameCache[109];
		_mapLegendCache[MapHelper.TileToLookup(630, 0)] = itemNameCache[5137];
		_mapLegendCache[MapHelper.TileToLookup(631, 0)] = itemNameCache[5138];
		_mapLegendCache[MapHelper.TileToLookup(13, 0)] = itemNameCache[31];
		_mapLegendCache[MapHelper.TileToLookup(14, 0)] = Language.GetText("MapObject.Table");
		_mapLegendCache[MapHelper.TileToLookup(469, 0)] = Language.GetText("MapObject.Table");
		_mapLegendCache[MapHelper.TileToLookup(486, 0)] = itemNameCache[4063];
		_mapLegendCache[MapHelper.TileToLookup(487, 0)] = itemNameCache[4064];
		_mapLegendCache[MapHelper.TileToLookup(487, 1)] = itemNameCache[4065];
		_mapLegendCache[MapHelper.TileToLookup(489, 0)] = itemNameCache[4074];
		_mapLegendCache[MapHelper.TileToLookup(490, 0)] = itemNameCache[4075];
		_mapLegendCache[MapHelper.TileToLookup(15, 0)] = Language.GetText("MapObject.Chair");
		_mapLegendCache[MapHelper.TileToLookup(15, 1)] = Language.GetText("MapObject.Toilet");
		_mapLegendCache[MapHelper.TileToLookup(16, 0)] = Language.GetText("MapObject.Anvil");
		_mapLegendCache[MapHelper.TileToLookup(17, 0)] = itemNameCache[33];
		_mapLegendCache[MapHelper.TileToLookup(18, 0)] = itemNameCache[36];
		_mapLegendCache[MapHelper.TileToLookup(20, 0)] = Language.GetText("MapObject.Sapling");
		_mapLegendCache[MapHelper.TileToLookup(590, 0)] = Language.GetText("MapObject.Sapling");
		_mapLegendCache[MapHelper.TileToLookup(595, 0)] = Language.GetText("MapObject.Sapling");
		_mapLegendCache[MapHelper.TileToLookup(615, 0)] = Language.GetText("MapObject.Sapling");
		_mapLegendCache[MapHelper.TileToLookup(21, 0)] = itemNameCache[48];
		_mapLegendCache[MapHelper.TileToLookup(467, 0)] = itemNameCache[48];
		_mapLegendCache[MapHelper.TileToLookup(22, 0)] = Language.GetText("MapObject.Demonite");
		_mapLegendCache[MapHelper.TileToLookup(26, 0)] = Language.GetText("MapObject.DemonAltar");
		_mapLegendCache[MapHelper.TileToLookup(26, 1)] = Language.GetText("MapObject.CrimsonAltar");
		_mapLegendCache[MapHelper.TileToLookup(27, 0)] = itemNameCache[63];
		_mapLegendCache[MapHelper.TileToLookup(407, 0)] = Language.GetText("MapObject.Fossil");
		_mapLegendCache[MapHelper.TileToLookup(412, 0)] = itemNameCache[3549];
		_mapLegendCache[MapHelper.TileToLookup(441, 0)] = itemNameCache[48];
		_mapLegendCache[MapHelper.TileToLookup(468, 0)] = itemNameCache[48];
		_mapLegendCache[MapHelper.TileToLookup(404, 0)] = Language.GetText("MapObject.DesertFossil");
		_mapLegendCache[MapHelper.TileToLookup(654, 0)] = itemNameCache[5327];
		_mapLegendCache[MapHelper.TileToLookup(695, 0)] = itemNameCache[5467];
		_mapLegendCache[MapHelper.TileToLookup(695, 1)] = itemNameCache[5468];
		_mapLegendCache[MapHelper.TileToLookup(696, 0)] = itemNameCache[5469];
		_mapLegendCache[MapHelper.TileToLookup(696, 1)] = itemNameCache[5470];
		for (int k = 0; k < 9; k++)
		{
			_mapLegendCache[MapHelper.TileToLookup(28, k)] = Language.GetText("MapObject.Pot");
			_mapLegendCache[MapHelper.TileToLookup(653, k)] = Language.GetText("MapObject.Pot");
		}
		_mapLegendCache[MapHelper.TileToLookup(37, 0)] = itemNameCache[116];
		_mapLegendCache[MapHelper.TileToLookup(29, 0)] = itemNameCache[87];
		_mapLegendCache[MapHelper.TileToLookup(31, 0)] = itemNameCache[115];
		_mapLegendCache[MapHelper.TileToLookup(31, 1)] = itemNameCache[3062];
		_mapLegendCache[MapHelper.TileToLookup(32, 0)] = Language.GetText("MapObject.Thorns");
		_mapLegendCache[MapHelper.TileToLookup(33, 0)] = itemNameCache[105];
		_mapLegendCache[MapHelper.TileToLookup(34, 0)] = Language.GetText("MapObject.Chandelier");
		_mapLegendCache[MapHelper.TileToLookup(35, 0)] = itemNameCache[1813];
		_mapLegendCache[MapHelper.TileToLookup(36, 0)] = itemNameCache[1869];
		_mapLegendCache[MapHelper.TileToLookup(476, 0)] = itemNameCache[4040];
		_mapLegendCache[MapHelper.TileToLookup(42, 0)] = Language.GetText("MapObject.Lantern");
		_mapLegendCache[MapHelper.TileToLookup(48, 0)] = itemNameCache[147];
		_mapLegendCache[MapHelper.TileToLookup(49, 0)] = itemNameCache[148];
		_mapLegendCache[MapHelper.TileToLookup(50, 0)] = itemNameCache[149];
		_mapLegendCache[MapHelper.TileToLookup(707, 0)] = itemNameCache[149];
		_mapLegendCache[MapHelper.TileToLookup(51, 0)] = Language.GetText("MapObject.Web");
		_mapLegendCache[MapHelper.TileToLookup(697, 0)] = Language.GetText("MapObject.Web");
		_mapLegendCache[MapHelper.TileToLookup(55, 0)] = itemNameCache[171];
		_mapLegendCache[MapHelper.TileToLookup(454, 0)] = itemNameCache[3746];
		_mapLegendCache[MapHelper.TileToLookup(455, 0)] = itemNameCache[3747];
		_mapLegendCache[MapHelper.TileToLookup(452, 0)] = itemNameCache[3742];
		_mapLegendCache[MapHelper.TileToLookup(456, 0)] = itemNameCache[3748];
		_mapLegendCache[MapHelper.TileToLookup(453, 0)] = itemNameCache[3744];
		_mapLegendCache[MapHelper.TileToLookup(453, 1)] = itemNameCache[3745];
		_mapLegendCache[MapHelper.TileToLookup(453, 2)] = itemNameCache[3743];
		_mapLegendCache[MapHelper.TileToLookup(573, 0)] = itemNameCache[4710];
		_mapLegendCache[MapHelper.TileToLookup(63, 0)] = itemNameCache[177];
		_mapLegendCache[MapHelper.TileToLookup(64, 0)] = itemNameCache[178];
		_mapLegendCache[MapHelper.TileToLookup(65, 0)] = itemNameCache[179];
		_mapLegendCache[MapHelper.TileToLookup(66, 0)] = itemNameCache[180];
		_mapLegendCache[MapHelper.TileToLookup(67, 0)] = itemNameCache[181];
		_mapLegendCache[MapHelper.TileToLookup(68, 0)] = itemNameCache[182];
		_mapLegendCache[MapHelper.TileToLookup(566, 0)] = itemNameCache[999];
		_mapLegendCache[MapHelper.TileToLookup(69, 0)] = Language.GetText("MapObject.Thorn");
		_mapLegendCache[MapHelper.TileToLookup(72, 0)] = Language.GetText("MapObject.GiantMushroom");
		_mapLegendCache[MapHelper.TileToLookup(77, 0)] = itemNameCache[221];
		_mapLegendCache[MapHelper.TileToLookup(78, 0)] = itemNameCache[222];
		_mapLegendCache[MapHelper.TileToLookup(79, 0)] = itemNameCache[224];
		_mapLegendCache[MapHelper.TileToLookup(80, 0)] = itemNameCache[276];
		_mapLegendCache[MapHelper.TileToLookup(81, 0)] = itemNameCache[275];
		_mapLegendCache[MapHelper.TileToLookup(82, 0)] = itemNameCache[313];
		_mapLegendCache[MapHelper.TileToLookup(82, 1)] = itemNameCache[314];
		_mapLegendCache[MapHelper.TileToLookup(82, 2)] = itemNameCache[315];
		_mapLegendCache[MapHelper.TileToLookup(82, 3)] = itemNameCache[316];
		_mapLegendCache[MapHelper.TileToLookup(82, 4)] = itemNameCache[317];
		_mapLegendCache[MapHelper.TileToLookup(82, 5)] = itemNameCache[318];
		_mapLegendCache[MapHelper.TileToLookup(82, 6)] = itemNameCache[2358];
		_mapLegendCache[MapHelper.TileToLookup(83, 0)] = itemNameCache[313];
		_mapLegendCache[MapHelper.TileToLookup(83, 1)] = itemNameCache[314];
		_mapLegendCache[MapHelper.TileToLookup(83, 2)] = itemNameCache[315];
		_mapLegendCache[MapHelper.TileToLookup(83, 3)] = itemNameCache[316];
		_mapLegendCache[MapHelper.TileToLookup(83, 4)] = itemNameCache[317];
		_mapLegendCache[MapHelper.TileToLookup(83, 5)] = itemNameCache[318];
		_mapLegendCache[MapHelper.TileToLookup(83, 6)] = itemNameCache[2358];
		_mapLegendCache[MapHelper.TileToLookup(84, 0)] = itemNameCache[313];
		_mapLegendCache[MapHelper.TileToLookup(84, 1)] = itemNameCache[314];
		_mapLegendCache[MapHelper.TileToLookup(84, 2)] = itemNameCache[315];
		_mapLegendCache[MapHelper.TileToLookup(84, 3)] = itemNameCache[316];
		_mapLegendCache[MapHelper.TileToLookup(84, 4)] = itemNameCache[317];
		_mapLegendCache[MapHelper.TileToLookup(84, 5)] = itemNameCache[318];
		_mapLegendCache[MapHelper.TileToLookup(84, 6)] = itemNameCache[2358];
		_mapLegendCache[MapHelper.TileToLookup(85, 0)] = itemNameCache[321];
		_mapLegendCache[MapHelper.TileToLookup(86, 0)] = itemNameCache[332];
		_mapLegendCache[MapHelper.TileToLookup(87, 0)] = itemNameCache[333];
		_mapLegendCache[MapHelper.TileToLookup(88, 0)] = itemNameCache[334];
		_mapLegendCache[MapHelper.TileToLookup(89, 0)] = itemNameCache[335];
		_mapLegendCache[MapHelper.TileToLookup(89, 1)] = itemNameCache[2397];
		_mapLegendCache[MapHelper.TileToLookup(89, 2)] = itemNameCache[4993];
		_mapLegendCache[MapHelper.TileToLookup(90, 0)] = itemNameCache[336];
		_mapLegendCache[MapHelper.TileToLookup(91, 0)] = Language.GetText("MapObject.Banner");
		_mapLegendCache[MapHelper.TileToLookup(92, 0)] = itemNameCache[341];
		_mapLegendCache[MapHelper.TileToLookup(93, 0)] = Language.GetText("MapObject.FloorLamp");
		_mapLegendCache[MapHelper.TileToLookup(94, 0)] = itemNameCache[352];
		_mapLegendCache[MapHelper.TileToLookup(95, 0)] = itemNameCache[344];
		_mapLegendCache[MapHelper.TileToLookup(96, 0)] = itemNameCache[345];
		_mapLegendCache[MapHelper.TileToLookup(97, 0)] = itemNameCache[346];
		_mapLegendCache[MapHelper.TileToLookup(98, 0)] = itemNameCache[347];
		_mapLegendCache[MapHelper.TileToLookup(100, 0)] = itemNameCache[349];
		_mapLegendCache[MapHelper.TileToLookup(101, 0)] = itemNameCache[354];
		_mapLegendCache[MapHelper.TileToLookup(102, 0)] = itemNameCache[355];
		_mapLegendCache[MapHelper.TileToLookup(103, 0)] = itemNameCache[356];
		_mapLegendCache[MapHelper.TileToLookup(104, 0)] = itemNameCache[359];
		_mapLegendCache[MapHelper.TileToLookup(105, 0)] = Language.GetText("MapObject.Statue");
		_mapLegendCache[MapHelper.TileToLookup(105, 2)] = Language.GetText("MapObject.Vase");
		_mapLegendCache[MapHelper.TileToLookup(106, 0)] = itemNameCache[363];
		_mapLegendCache[MapHelper.TileToLookup(107, 0)] = Language.GetText("MapObject.Cobalt");
		_mapLegendCache[MapHelper.TileToLookup(108, 0)] = Language.GetText("MapObject.Mythril");
		_mapLegendCache[MapHelper.TileToLookup(111, 0)] = Language.GetText("MapObject.Adamantite");
		_mapLegendCache[MapHelper.TileToLookup(114, 0)] = itemNameCache[398];
		_mapLegendCache[MapHelper.TileToLookup(125, 0)] = itemNameCache[487];
		_mapLegendCache[MapHelper.TileToLookup(128, 0)] = itemNameCache[498];
		_mapLegendCache[MapHelper.TileToLookup(129, 0)] = itemNameCache[502];
		_mapLegendCache[MapHelper.TileToLookup(129, 1)] = itemNameCache[4988];
		_mapLegendCache[MapHelper.TileToLookup(132, 0)] = itemNameCache[513];
		_mapLegendCache[MapHelper.TileToLookup(411, 0)] = itemNameCache[3545];
		_mapLegendCache[MapHelper.TileToLookup(133, 0)] = itemNameCache[524];
		_mapLegendCache[MapHelper.TileToLookup(133, 1)] = itemNameCache[1221];
		_mapLegendCache[MapHelper.TileToLookup(134, 0)] = itemNameCache[525];
		_mapLegendCache[MapHelper.TileToLookup(134, 1)] = itemNameCache[1220];
		_mapLegendCache[MapHelper.TileToLookup(136, 0)] = itemNameCache[538];
		_mapLegendCache[MapHelper.TileToLookup(137, 0)] = Language.GetText("MapObject.Trap");
		_mapLegendCache[MapHelper.TileToLookup(138, 0)] = itemNameCache[540];
		_mapLegendCache[MapHelper.TileToLookup(139, 0)] = itemNameCache[576];
		_mapLegendCache[MapHelper.TileToLookup(142, 0)] = itemNameCache[581];
		_mapLegendCache[MapHelper.TileToLookup(143, 0)] = itemNameCache[582];
		_mapLegendCache[MapHelper.TileToLookup(144, 0)] = Language.GetText("MapObject.Timer");
		_mapLegendCache[MapHelper.TileToLookup(149, 0)] = Language.GetText("MapObject.ChristmasLight");
		_mapLegendCache[MapHelper.TileToLookup(166, 0)] = Language.GetText("MapObject.Tin");
		_mapLegendCache[MapHelper.TileToLookup(167, 0)] = Language.GetText("MapObject.Lead");
		_mapLegendCache[MapHelper.TileToLookup(168, 0)] = Language.GetText("MapObject.Tungsten");
		_mapLegendCache[MapHelper.TileToLookup(169, 0)] = Language.GetText("MapObject.Platinum");
		_mapLegendCache[MapHelper.TileToLookup(170, 0)] = Language.GetText("MapObject.PineTree");
		_mapLegendCache[MapHelper.TileToLookup(171, 0)] = itemNameCache[1873];
		_mapLegendCache[MapHelper.TileToLookup(172, 0)] = Language.GetText("MapObject.Sink");
		_mapLegendCache[MapHelper.TileToLookup(173, 0)] = itemNameCache[349];
		_mapLegendCache[MapHelper.TileToLookup(174, 0)] = itemNameCache[713];
		_mapLegendCache[MapHelper.TileToLookup(178, 0)] = itemNameCache[181];
		_mapLegendCache[MapHelper.TileToLookup(178, 1)] = itemNameCache[180];
		_mapLegendCache[MapHelper.TileToLookup(178, 2)] = itemNameCache[177];
		_mapLegendCache[MapHelper.TileToLookup(178, 3)] = itemNameCache[179];
		_mapLegendCache[MapHelper.TileToLookup(178, 4)] = itemNameCache[178];
		_mapLegendCache[MapHelper.TileToLookup(178, 5)] = itemNameCache[182];
		_mapLegendCache[MapHelper.TileToLookup(178, 6)] = itemNameCache[999];
		_mapLegendCache[MapHelper.TileToLookup(191, 0)] = Language.GetText("MapObject.LivingWood");
		_mapLegendCache[MapHelper.TileToLookup(204, 0)] = Language.GetText("MapObject.Crimtane");
		_mapLegendCache[MapHelper.TileToLookup(207, 0)] = Language.GetText("MapObject.WaterFountain");
		_mapLegendCache[MapHelper.TileToLookup(209, 0)] = itemNameCache[928];
		_mapLegendCache[MapHelper.TileToLookup(211, 0)] = Language.GetText("MapObject.Chlorophyte");
		_mapLegendCache[MapHelper.TileToLookup(212, 0)] = Language.GetText("MapObject.Turret");
		_mapLegendCache[MapHelper.TileToLookup(213, 0)] = itemNameCache[965];
		_mapLegendCache[MapHelper.TileToLookup(214, 0)] = itemNameCache[85];
		_mapLegendCache[MapHelper.TileToLookup(215, 0)] = itemNameCache[966];
		_mapLegendCache[MapHelper.TileToLookup(216, 0)] = Language.GetText("MapObject.Rocket");
		_mapLegendCache[MapHelper.TileToLookup(217, 0)] = itemNameCache[995];
		_mapLegendCache[MapHelper.TileToLookup(218, 0)] = itemNameCache[996];
		_mapLegendCache[MapHelper.TileToLookup(219, 0)] = Language.GetText("MapObject.SiltExtractinator");
		_mapLegendCache[MapHelper.TileToLookup(642, 0)] = Language.GetText("MapObject.ChlorophyteExtractinator");
		_mapLegendCache[MapHelper.TileToLookup(220, 0)] = itemNameCache[998];
		_mapLegendCache[MapHelper.TileToLookup(221, 0)] = Language.GetText("MapObject.Palladium");
		_mapLegendCache[MapHelper.TileToLookup(222, 0)] = Language.GetText("MapObject.Orichalcum");
		_mapLegendCache[MapHelper.TileToLookup(223, 0)] = Language.GetText("MapObject.Titanium");
		_mapLegendCache[MapHelper.TileToLookup(227, 0)] = itemNameCache[1107];
		_mapLegendCache[MapHelper.TileToLookup(227, 1)] = itemNameCache[1108];
		_mapLegendCache[MapHelper.TileToLookup(227, 2)] = itemNameCache[1109];
		_mapLegendCache[MapHelper.TileToLookup(227, 3)] = itemNameCache[1110];
		_mapLegendCache[MapHelper.TileToLookup(227, 4)] = itemNameCache[1111];
		_mapLegendCache[MapHelper.TileToLookup(227, 5)] = itemNameCache[1112];
		_mapLegendCache[MapHelper.TileToLookup(227, 6)] = itemNameCache[1113];
		_mapLegendCache[MapHelper.TileToLookup(227, 7)] = itemNameCache[1114];
		_mapLegendCache[MapHelper.TileToLookup(227, 8)] = itemNameCache[3385];
		_mapLegendCache[MapHelper.TileToLookup(227, 9)] = itemNameCache[3386];
		_mapLegendCache[MapHelper.TileToLookup(227, 10)] = itemNameCache[3387];
		_mapLegendCache[MapHelper.TileToLookup(227, 11)] = itemNameCache[3388];
		_mapLegendCache[MapHelper.TileToLookup(228, 0)] = itemNameCache[1120];
		_mapLegendCache[MapHelper.TileToLookup(231, 0)] = Language.GetText("MapObject.Larva");
		_mapLegendCache[MapHelper.TileToLookup(232, 0)] = itemNameCache[1150];
		_mapLegendCache[MapHelper.TileToLookup(235, 0)] = itemNameCache[1263];
		_mapLegendCache[MapHelper.TileToLookup(624, 0)] = itemNameCache[5114];
		_mapLegendCache[MapHelper.TileToLookup(700, 0)] = itemNameCache[5114];
		_mapLegendCache[MapHelper.TileToLookup(656, 0)] = itemNameCache[5333];
		_mapLegendCache[MapHelper.TileToLookup(701, 0)] = itemNameCache[5333];
		_mapLegendCache[MapHelper.TileToLookup(236, 0)] = itemNameCache[1291];
		_mapLegendCache[MapHelper.TileToLookup(702, 0)] = itemNameCache[1291];
		_mapLegendCache[MapHelper.TileToLookup(237, 0)] = itemNameCache[1292];
		_mapLegendCache[MapHelper.TileToLookup(238, 0)] = Language.GetText("MapObject.PlanterasBulb");
		_mapLegendCache[MapHelper.TileToLookup(239, 0)] = Language.GetText("MapObject.MetalBar");
		_mapLegendCache[MapHelper.TileToLookup(240, 0)] = Language.GetText("MapObject.Trophy");
		_mapLegendCache[MapHelper.TileToLookup(240, 1)] = Language.GetText("MapObject.Painting");
		_mapLegendCache[MapHelper.TileToLookup(240, 2)] = _npcNameCache[21];
		_mapLegendCache[MapHelper.TileToLookup(240, 3)] = Language.GetText("MapObject.ItemRack");
		_mapLegendCache[MapHelper.TileToLookup(240, 4)] = itemNameCache[2442];
		_mapLegendCache[MapHelper.TileToLookup(241, 0)] = itemNameCache[1417];
		_mapLegendCache[MapHelper.TileToLookup(242, 0)] = Language.GetText("MapObject.Painting");
		_mapLegendCache[MapHelper.TileToLookup(242, 1)] = Language.GetText("MapObject.AnimalSkin");
		_mapLegendCache[MapHelper.TileToLookup(243, 0)] = itemNameCache[1430];
		_mapLegendCache[MapHelper.TileToLookup(244, 0)] = itemNameCache[1449];
		_mapLegendCache[MapHelper.TileToLookup(245, 0)] = Language.GetText("MapObject.Picture");
		_mapLegendCache[MapHelper.TileToLookup(246, 0)] = Language.GetText("MapObject.Picture");
		_mapLegendCache[MapHelper.TileToLookup(247, 0)] = itemNameCache[1551];
		_mapLegendCache[MapHelper.TileToLookup(254, 0)] = itemNameCache[1725];
		_mapLegendCache[MapHelper.TileToLookup(269, 0)] = itemNameCache[1989];
		_mapLegendCache[MapHelper.TileToLookup(475, 0)] = itemNameCache[3977];
		_mapLegendCache[MapHelper.TileToLookup(597, 0)] = itemNameCache[4876];
		_mapLegendCache[MapHelper.TileToLookup(597, 1)] = itemNameCache[4875];
		_mapLegendCache[MapHelper.TileToLookup(597, 2)] = itemNameCache[4916];
		_mapLegendCache[MapHelper.TileToLookup(597, 3)] = itemNameCache[4917];
		_mapLegendCache[MapHelper.TileToLookup(597, 4)] = itemNameCache[4918];
		_mapLegendCache[MapHelper.TileToLookup(597, 5)] = itemNameCache[4919];
		_mapLegendCache[MapHelper.TileToLookup(597, 6)] = itemNameCache[4920];
		_mapLegendCache[MapHelper.TileToLookup(597, 7)] = itemNameCache[4921];
		_mapLegendCache[MapHelper.TileToLookup(597, 8)] = itemNameCache[4951];
		_mapLegendCache[MapHelper.TileToLookup(597, 9)] = itemNameCache[5652];
		_mapLegendCache[MapHelper.TileToLookup(597, 10)] = itemNameCache[5653];
		_mapLegendCache[MapHelper.TileToLookup(617, 0)] = Language.GetText("MapObject.Relic");
		_mapLegendCache[MapHelper.TileToLookup(621, 0)] = itemNameCache[3750];
		_mapLegendCache[MapHelper.TileToLookup(622, 0)] = itemNameCache[5008];
		_mapLegendCache[MapHelper.TileToLookup(270, 0)] = itemNameCache[1993];
		_mapLegendCache[MapHelper.TileToLookup(271, 0)] = itemNameCache[2005];
		_mapLegendCache[MapHelper.TileToLookup(581, 0)] = itemNameCache[4848];
		_mapLegendCache[MapHelper.TileToLookup(660, 0)] = itemNameCache[5351];
		_mapLegendCache[MapHelper.TileToLookup(275, 0)] = itemNameCache[2162];
		_mapLegendCache[MapHelper.TileToLookup(276, 0)] = itemNameCache[2163];
		_mapLegendCache[MapHelper.TileToLookup(277, 0)] = itemNameCache[2164];
		_mapLegendCache[MapHelper.TileToLookup(278, 0)] = itemNameCache[2165];
		_mapLegendCache[MapHelper.TileToLookup(279, 0)] = itemNameCache[2166];
		_mapLegendCache[MapHelper.TileToLookup(280, 0)] = itemNameCache[2167];
		_mapLegendCache[MapHelper.TileToLookup(281, 0)] = itemNameCache[2168];
		_mapLegendCache[MapHelper.TileToLookup(632, 0)] = itemNameCache[5213];
		_mapLegendCache[MapHelper.TileToLookup(640, 0)] = itemNameCache[5301];
		_mapLegendCache[MapHelper.TileToLookup(643, 0)] = itemNameCache[5314];
		_mapLegendCache[MapHelper.TileToLookup(644, 0)] = itemNameCache[5315];
		_mapLegendCache[MapHelper.TileToLookup(645, 0)] = itemNameCache[5316];
		_mapLegendCache[MapHelper.TileToLookup(282, 0)] = itemNameCache[250];
		_mapLegendCache[MapHelper.TileToLookup(543, 0)] = itemNameCache[4398];
		_mapLegendCache[MapHelper.TileToLookup(598, 0)] = itemNameCache[4880];
		_mapLegendCache[MapHelper.TileToLookup(413, 0)] = itemNameCache[3565];
		_mapLegendCache[MapHelper.TileToLookup(710, 0)] = itemNameCache[5512];
		_mapLegendCache[MapHelper.TileToLookup(283, 0)] = itemNameCache[2172];
		_mapLegendCache[MapHelper.TileToLookup(285, 0)] = itemNameCache[2174];
		_mapLegendCache[MapHelper.TileToLookup(286, 0)] = itemNameCache[2175];
		_mapLegendCache[MapHelper.TileToLookup(582, 0)] = itemNameCache[4850];
		_mapLegendCache[MapHelper.TileToLookup(287, 0)] = itemNameCache[2177];
		_mapLegendCache[MapHelper.TileToLookup(288, 0)] = itemNameCache[2178];
		_mapLegendCache[MapHelper.TileToLookup(289, 0)] = itemNameCache[2179];
		_mapLegendCache[MapHelper.TileToLookup(290, 0)] = itemNameCache[2180];
		_mapLegendCache[MapHelper.TileToLookup(291, 0)] = itemNameCache[2181];
		_mapLegendCache[MapHelper.TileToLookup(292, 0)] = itemNameCache[2182];
		_mapLegendCache[MapHelper.TileToLookup(293, 0)] = itemNameCache[2183];
		_mapLegendCache[MapHelper.TileToLookup(294, 0)] = itemNameCache[2184];
		_mapLegendCache[MapHelper.TileToLookup(295, 0)] = itemNameCache[2185];
		_mapLegendCache[MapHelper.TileToLookup(580, 0)] = itemNameCache[4846];
		_mapLegendCache[MapHelper.TileToLookup(620, 0)] = itemNameCache[4964];
		_mapLegendCache[MapHelper.TileToLookup(619, 0)] = itemNameCache[4963];
		_mapLegendCache[MapHelper.TileToLookup(296, 0)] = itemNameCache[2186];
		_mapLegendCache[MapHelper.TileToLookup(297, 0)] = itemNameCache[2187];
		_mapLegendCache[MapHelper.TileToLookup(298, 0)] = itemNameCache[2190];
		_mapLegendCache[MapHelper.TileToLookup(299, 0)] = itemNameCache[2191];
		_mapLegendCache[MapHelper.TileToLookup(300, 0)] = itemNameCache[2192];
		_mapLegendCache[MapHelper.TileToLookup(301, 0)] = itemNameCache[2193];
		_mapLegendCache[MapHelper.TileToLookup(302, 0)] = itemNameCache[2194];
		_mapLegendCache[MapHelper.TileToLookup(303, 0)] = itemNameCache[2195];
		_mapLegendCache[MapHelper.TileToLookup(304, 0)] = itemNameCache[2196];
		_mapLegendCache[MapHelper.TileToLookup(305, 0)] = itemNameCache[2197];
		_mapLegendCache[MapHelper.TileToLookup(306, 0)] = itemNameCache[2198];
		_mapLegendCache[MapHelper.TileToLookup(307, 0)] = itemNameCache[2203];
		_mapLegendCache[MapHelper.TileToLookup(308, 0)] = itemNameCache[2204];
		_mapLegendCache[MapHelper.TileToLookup(309, 0)] = itemNameCache[2206];
		_mapLegendCache[MapHelper.TileToLookup(310, 0)] = itemNameCache[2207];
		_mapLegendCache[MapHelper.TileToLookup(391, 0)] = itemNameCache[3254];
		_mapLegendCache[MapHelper.TileToLookup(316, 0)] = itemNameCache[2439];
		_mapLegendCache[MapHelper.TileToLookup(317, 0)] = itemNameCache[2440];
		_mapLegendCache[MapHelper.TileToLookup(318, 0)] = itemNameCache[2441];
		_mapLegendCache[MapHelper.TileToLookup(319, 0)] = itemNameCache[2490];
		_mapLegendCache[MapHelper.TileToLookup(320, 0)] = itemNameCache[2496];
		_mapLegendCache[MapHelper.TileToLookup(323, 0)] = Language.GetText("MapObject.PalmTree");
		_mapLegendCache[MapHelper.TileToLookup(314, 0)] = itemNameCache[2340];
		_mapLegendCache[MapHelper.TileToLookup(353, 0)] = itemNameCache[2996];
		_mapLegendCache[MapHelper.TileToLookup(354, 0)] = itemNameCache[2999];
		_mapLegendCache[MapHelper.TileToLookup(355, 0)] = itemNameCache[3000];
		_mapLegendCache[MapHelper.TileToLookup(464, 0)] = itemNameCache[3814];
		_mapLegendCache[MapHelper.TileToLookup(699, 0)] = itemNameCache[5482];
		_mapLegendCache[MapHelper.TileToLookup(356, 0)] = itemNameCache[3064];
		_mapLegendCache[MapHelper.TileToLookup(663, 0)] = itemNameCache[5381];
		_mapLegendCache[MapHelper.TileToLookup(567, 0)] = itemNameCache[4609];
		_mapLegendCache[MapHelper.TileToLookup(358, 0)] = itemNameCache[3070];
		_mapLegendCache[MapHelper.TileToLookup(359, 0)] = itemNameCache[3071];
		_mapLegendCache[MapHelper.TileToLookup(360, 0)] = itemNameCache[3072];
		_mapLegendCache[MapHelper.TileToLookup(361, 0)] = itemNameCache[3073];
		_mapLegendCache[MapHelper.TileToLookup(362, 0)] = itemNameCache[3074];
		_mapLegendCache[MapHelper.TileToLookup(363, 0)] = itemNameCache[3075];
		_mapLegendCache[MapHelper.TileToLookup(364, 0)] = itemNameCache[3076];
		_mapLegendCache[MapHelper.TileToLookup(414, 0)] = itemNameCache[3566];
		_mapLegendCache[MapHelper.TileToLookup(521, 0)] = itemNameCache[4327];
		_mapLegendCache[MapHelper.TileToLookup(522, 0)] = itemNameCache[4328];
		_mapLegendCache[MapHelper.TileToLookup(523, 0)] = itemNameCache[4329];
		_mapLegendCache[MapHelper.TileToLookup(524, 0)] = itemNameCache[4330];
		_mapLegendCache[MapHelper.TileToLookup(525, 0)] = itemNameCache[4331];
		_mapLegendCache[MapHelper.TileToLookup(526, 0)] = itemNameCache[4332];
		_mapLegendCache[MapHelper.TileToLookup(527, 0)] = itemNameCache[4333];
		_mapLegendCache[MapHelper.TileToLookup(542, 0)] = itemNameCache[4396];
		_mapLegendCache[MapHelper.TileToLookup(365, 0)] = itemNameCache[3077];
		_mapLegendCache[MapHelper.TileToLookup(366, 0)] = itemNameCache[3078];
		_mapLegendCache[MapHelper.TileToLookup(373, 0)] = Language.GetText("MapObject.DrippingWater");
		_mapLegendCache[MapHelper.TileToLookup(374, 0)] = Language.GetText("MapObject.DrippingLava");
		_mapLegendCache[MapHelper.TileToLookup(375, 0)] = Language.GetText("MapObject.DrippingHoney");
		_mapLegendCache[MapHelper.TileToLookup(709, 0)] = Language.GetText("MapObject.DrippingShimmer");
		_mapLegendCache[MapHelper.TileToLookup(461, 0)] = Language.GetText("MapObject.SandFlow");
		_mapLegendCache[MapHelper.TileToLookup(461, 1)] = Language.GetText("MapObject.SandFlow");
		_mapLegendCache[MapHelper.TileToLookup(461, 2)] = Language.GetText("MapObject.SandFlow");
		_mapLegendCache[MapHelper.TileToLookup(461, 3)] = Language.GetText("MapObject.SandFlow");
		_mapLegendCache[MapHelper.TileToLookup(377, 0)] = itemNameCache[3198];
		_mapLegendCache[MapHelper.TileToLookup(372, 0)] = itemNameCache[3117];
		_mapLegendCache[MapHelper.TileToLookup(646, 0)] = itemNameCache[5322];
		_mapLegendCache[MapHelper.TileToLookup(425, 0)] = itemNameCache[3617];
		_mapLegendCache[MapHelper.TileToLookup(420, 0)] = itemNameCache[3603];
		_mapLegendCache[MapHelper.TileToLookup(420, 1)] = itemNameCache[3604];
		_mapLegendCache[MapHelper.TileToLookup(420, 2)] = itemNameCache[3605];
		_mapLegendCache[MapHelper.TileToLookup(420, 3)] = itemNameCache[3606];
		_mapLegendCache[MapHelper.TileToLookup(420, 4)] = itemNameCache[3607];
		_mapLegendCache[MapHelper.TileToLookup(420, 5)] = itemNameCache[3608];
		_mapLegendCache[MapHelper.TileToLookup(423, 0)] = itemNameCache[3613];
		_mapLegendCache[MapHelper.TileToLookup(423, 1)] = itemNameCache[3614];
		_mapLegendCache[MapHelper.TileToLookup(423, 2)] = itemNameCache[3615];
		_mapLegendCache[MapHelper.TileToLookup(423, 3)] = itemNameCache[3726];
		_mapLegendCache[MapHelper.TileToLookup(423, 4)] = itemNameCache[3727];
		_mapLegendCache[MapHelper.TileToLookup(423, 5)] = itemNameCache[3728];
		_mapLegendCache[MapHelper.TileToLookup(423, 6)] = itemNameCache[3729];
		_mapLegendCache[MapHelper.TileToLookup(440, 0)] = itemNameCache[3644];
		_mapLegendCache[MapHelper.TileToLookup(440, 1)] = itemNameCache[3645];
		_mapLegendCache[MapHelper.TileToLookup(440, 2)] = itemNameCache[3646];
		_mapLegendCache[MapHelper.TileToLookup(440, 3)] = itemNameCache[3647];
		_mapLegendCache[MapHelper.TileToLookup(440, 4)] = itemNameCache[3648];
		_mapLegendCache[MapHelper.TileToLookup(440, 5)] = itemNameCache[3649];
		_mapLegendCache[MapHelper.TileToLookup(440, 6)] = itemNameCache[3650];
		_mapLegendCache[MapHelper.TileToLookup(443, 0)] = itemNameCache[3722];
		_mapLegendCache[MapHelper.TileToLookup(429, 0)] = itemNameCache[3629];
		_mapLegendCache[MapHelper.TileToLookup(424, 0)] = itemNameCache[3616];
		_mapLegendCache[MapHelper.TileToLookup(444, 0)] = Language.GetText("MapObject.BeeHive");
		_mapLegendCache[MapHelper.TileToLookup(466, 0)] = itemNameCache[3816];
		_mapLegendCache[MapHelper.TileToLookup(463, 0)] = itemNameCache[3813];
		_mapLegendCache[MapHelper.TileToLookup(491, 0)] = itemNameCache[4076];
		_mapLegendCache[MapHelper.TileToLookup(494, 0)] = itemNameCache[4089];
		_mapLegendCache[MapHelper.TileToLookup(499, 0)] = itemNameCache[4142];
		_mapLegendCache[MapHelper.TileToLookup(488, 0)] = Language.GetText("MapObject.FallenLog");
		_mapLegendCache[MapHelper.TileToLookup(704, 0)] = Language.GetText("MapObject.FallenLog");
		_mapLegendCache[MapHelper.TileToLookup(505, 0)] = itemNameCache[4275];
		_mapLegendCache[MapHelper.TileToLookup(521, 0)] = itemNameCache[4327];
		_mapLegendCache[MapHelper.TileToLookup(522, 0)] = itemNameCache[4328];
		_mapLegendCache[MapHelper.TileToLookup(523, 0)] = itemNameCache[4329];
		_mapLegendCache[MapHelper.TileToLookup(524, 0)] = itemNameCache[4330];
		_mapLegendCache[MapHelper.TileToLookup(525, 0)] = itemNameCache[4331];
		_mapLegendCache[MapHelper.TileToLookup(526, 0)] = itemNameCache[4332];
		_mapLegendCache[MapHelper.TileToLookup(527, 0)] = itemNameCache[4333];
		_mapLegendCache[MapHelper.TileToLookup(531, 0)] = Language.GetText("MapObject.Statue");
		_mapLegendCache[MapHelper.TileToLookup(349, 0)] = Language.GetText("MapObject.Statue");
		_mapLegendCache[MapHelper.TileToLookup(532, 0)] = itemNameCache[4364];
		_mapLegendCache[MapHelper.TileToLookup(538, 0)] = itemNameCache[4380];
		_mapLegendCache[MapHelper.TileToLookup(544, 0)] = itemNameCache[4399];
		_mapLegendCache[MapHelper.TileToLookup(629, 0)] = itemNameCache[5133];
		_mapLegendCache[MapHelper.TileToLookup(506, 0)] = itemNameCache[4276];
		_mapLegendCache[MapHelper.TileToLookup(545, 0)] = itemNameCache[4420];
		_mapLegendCache[MapHelper.TileToLookup(550, 0)] = itemNameCache[4461];
		_mapLegendCache[MapHelper.TileToLookup(551, 0)] = itemNameCache[4462];
		_mapLegendCache[MapHelper.TileToLookup(533, 0)] = itemNameCache[4376];
		_mapLegendCache[MapHelper.TileToLookup(553, 0)] = itemNameCache[4473];
		_mapLegendCache[MapHelper.TileToLookup(554, 0)] = itemNameCache[4474];
		_mapLegendCache[MapHelper.TileToLookup(555, 0)] = itemNameCache[4475];
		_mapLegendCache[MapHelper.TileToLookup(556, 0)] = itemNameCache[4476];
		_mapLegendCache[MapHelper.TileToLookup(558, 0)] = itemNameCache[4481];
		_mapLegendCache[MapHelper.TileToLookup(559, 0)] = itemNameCache[4483];
		_mapLegendCache[MapHelper.TileToLookup(599, 0)] = itemNameCache[4882];
		_mapLegendCache[MapHelper.TileToLookup(600, 0)] = itemNameCache[4883];
		_mapLegendCache[MapHelper.TileToLookup(601, 0)] = itemNameCache[4884];
		_mapLegendCache[MapHelper.TileToLookup(602, 0)] = itemNameCache[4885];
		_mapLegendCache[MapHelper.TileToLookup(603, 0)] = itemNameCache[4886];
		_mapLegendCache[MapHelper.TileToLookup(604, 0)] = itemNameCache[4887];
		_mapLegendCache[MapHelper.TileToLookup(605, 0)] = itemNameCache[4888];
		_mapLegendCache[MapHelper.TileToLookup(606, 0)] = itemNameCache[4889];
		_mapLegendCache[MapHelper.TileToLookup(607, 0)] = itemNameCache[4890];
		_mapLegendCache[MapHelper.TileToLookup(608, 0)] = itemNameCache[4891];
		_mapLegendCache[MapHelper.TileToLookup(609, 0)] = itemNameCache[4892];
		_mapLegendCache[MapHelper.TileToLookup(610, 0)] = itemNameCache[4893];
		_mapLegendCache[MapHelper.TileToLookup(611, 0)] = itemNameCache[4894];
		_mapLegendCache[MapHelper.TileToLookup(612, 0)] = itemNameCache[4895];
		_mapLegendCache[MapHelper.TileToLookup(560, 0)] = itemNameCache[4599];
		_mapLegendCache[MapHelper.TileToLookup(560, 1)] = itemNameCache[4600];
		_mapLegendCache[MapHelper.TileToLookup(560, 2)] = itemNameCache[4601];
		_mapLegendCache[MapHelper.TileToLookup(568, 0)] = itemNameCache[4655];
		_mapLegendCache[MapHelper.TileToLookup(569, 0)] = itemNameCache[4656];
		_mapLegendCache[MapHelper.TileToLookup(570, 0)] = itemNameCache[4657];
		_mapLegendCache[MapHelper.TileToLookup(572, 0)] = itemNameCache[4695];
		_mapLegendCache[MapHelper.TileToLookup(572, 1)] = itemNameCache[4696];
		_mapLegendCache[MapHelper.TileToLookup(572, 2)] = itemNameCache[4697];
		_mapLegendCache[MapHelper.TileToLookup(572, 3)] = itemNameCache[4698];
		_mapLegendCache[MapHelper.TileToLookup(572, 4)] = itemNameCache[4699];
		_mapLegendCache[MapHelper.TileToLookup(572, 5)] = itemNameCache[4700];
		_mapLegendCache[MapHelper.TileToLookup(497, 0)] = Language.GetText("MapObject.Toilet");
		_mapLegendCache[MapHelper.TileToLookup(751, 0)] = itemNameCache[5667];
		_mapLegendCache[MapHelper.TileToLookup(752, 0)] = itemNameCache[6142];
	}

	public static NetworkText CreateDeathMessage(string deadPlayerName, int plr = -1, int npc = -1, int proj = -1, int other = -1, int projType = 0, int plrItemType = 0)
	{
		NetworkText networkText = NetworkText.Empty;
		NetworkText networkText2 = NetworkText.Empty;
		NetworkText networkText3 = NetworkText.Empty;
		NetworkText networkText4 = NetworkText.Empty;
		if (proj >= 0)
		{
			networkText = NetworkText.FromKey(GetProjectileName(projType).Key);
		}
		if (npc >= 0)
		{
			networkText2 = Main.npc[npc].GetGivenOrTypeNetName();
		}
		if (plr >= 0 && plr < 255)
		{
			networkText3 = NetworkText.FromLiteral(Main.player[plr].name);
		}
		if (plrItemType >= 0)
		{
			networkText4 = NetworkText.FromKey(GetItemName(plrItemType).Key);
		}
		bool flag = networkText != NetworkText.Empty;
		bool flag2 = plr >= 0 && plr < 255;
		bool flag3 = networkText2 != NetworkText.Empty;
		NetworkText result = NetworkText.Empty;
		NetworkText empty = NetworkText.Empty;
		empty = NetworkText.FromKey(Language.RandomFromCategory("DeathTextGeneric").Key, deadPlayerName, Main.worldName);
		if (flag2)
		{
			result = NetworkText.FromKey("DeathSource.Player", empty, networkText3, flag ? networkText : networkText4);
		}
		else if (flag3)
		{
			result = NetworkText.FromKey("DeathSource.NPC", empty, networkText2);
		}
		else if (flag)
		{
			result = NetworkText.FromKey("DeathSource.Projectile", empty, networkText);
		}
		else
		{
			switch (other)
			{
			case 0:
				result = NetworkText.FromKey("DeathText.Fell_" + (Main.rand.Next(9) + 1), deadPlayerName);
				break;
			case 1:
				result = NetworkText.FromKey("DeathText.Drowned_" + (Main.rand.Next(7) + 1), deadPlayerName);
				break;
			case 2:
				result = NetworkText.FromKey("DeathText.Lava_" + (Main.rand.Next(5) + 1), deadPlayerName);
				break;
			case 3:
				result = NetworkText.FromKey("DeathText.Default", empty);
				break;
			case 4:
				result = NetworkText.FromKey("DeathText.Slain", deadPlayerName);
				break;
			case 5:
				result = NetworkText.FromKey("DeathText.Petrified_" + (Main.rand.Next(4) + 1), deadPlayerName);
				break;
			case 6:
				result = NetworkText.FromKey("DeathText.Stabbed", deadPlayerName);
				break;
			case 7:
				result = NetworkText.FromKey("DeathText.Suffocated_" + (Main.rand.Next(2) + 1), deadPlayerName);
				break;
			case 8:
				result = NetworkText.FromKey("DeathText.Burned_" + (Main.rand.Next(4) + 1), deadPlayerName);
				break;
			case 9:
				result = NetworkText.FromKey("DeathText.Poisoned", deadPlayerName);
				break;
			case 10:
				result = NetworkText.FromKey("DeathText.Electrocuted_" + (Main.rand.Next(4) + 1), deadPlayerName);
				break;
			case 11:
				result = NetworkText.FromKey("DeathText.TriedToEscape", deadPlayerName);
				break;
			case 12:
				result = NetworkText.FromKey("DeathText.WasLicked_" + (Main.rand.Next(2) + 1), deadPlayerName);
				break;
			case 13:
				result = NetworkText.FromKey("DeathText.Teleport_1", deadPlayerName);
				break;
			case 14:
				result = NetworkText.FromKey("DeathText.Teleport_2_Male", deadPlayerName);
				break;
			case 15:
				result = NetworkText.FromKey("DeathText.Teleport_2_Female", deadPlayerName);
				break;
			case 16:
				result = NetworkText.FromKey("DeathText.Inferno", deadPlayerName);
				break;
			case 17:
				result = NetworkText.FromKey("DeathText.DiedInTheDark", deadPlayerName);
				break;
			case 18:
				result = NetworkText.FromKey("DeathText.Starved_" + (Main.rand.Next(3) + 1), deadPlayerName);
				break;
			case 19:
				result = NetworkText.FromKey("DeathText.Space_" + (Main.rand.Next(5) + 1), deadPlayerName, Main.worldName);
				break;
			case 20:
				result = NetworkText.FromKey("DeathText.TeamTank", deadPlayerName);
				break;
			case 21:
				result = NetworkText.FromKey("DeathText.Underground_" + (Main.rand.Next(5) + 1), deadPlayerName, Main.worldName);
				break;
			case 22:
				result = NetworkText.FromKey("DeathText.VampireBurningInDaylight_" + (Main.rand.Next(6) + 1), deadPlayerName, Main.worldName);
				break;
			case 23:
				result = NetworkText.FromKey("DeathText.Spored", deadPlayerName);
				break;
			case 255:
				result = NetworkText.FromKey("DeathText.Slain", deadPlayerName);
				break;
			}
		}
		return result;
	}

	public static NetworkText GetInvasionWaveText(int wave, params short[] npcIds)
	{
		NetworkText[] array = new NetworkText[npcIds.Length + 1];
		for (int i = 0; i < npcIds.Length; i++)
		{
			array[i + 1] = NetworkText.FromKey(GetNPCName(npcIds[i]).Key);
		}
		switch (wave)
		{
		case -1:
			array[0] = NetworkText.FromKey("Game.FinalWave");
			break;
		case 1:
			array[0] = NetworkText.FromKey("Game.FirstWave");
			break;
		default:
			array[0] = NetworkText.FromKey("Game.Wave", wave);
			break;
		}
		string key = "Game.InvasionWave_Type" + npcIds.Length;
		object[] substitutions = array;
		return NetworkText.FromKey(key, substitutions);
	}

	public static string LocalizedDuration(TimeSpan time, bool abbreviated, bool showAllAvailableUnits)
	{
		string text = "";
		abbreviated |= !GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive;
		if (time.Days > 0)
		{
			int num = time.Days;
			if (!showAllAvailableUnits && time > TimeSpan.FromDays(1.0))
			{
				num++;
			}
			string text2 = text;
			object obj = num;
			string text3;
			if (abbreviated)
			{
				text3 = " " + Language.GetTextValue("Misc.ShortDays");
			}
			else
			{
				text3 = ((num == 1) ? " day" : " days");
			}
			text = string.Concat(text2, obj, text3);
			if (!showAllAvailableUnits)
			{
				return text;
			}
			text += " ";
		}
		if (time.Hours > 0)
		{
			int num2 = time.Hours;
			if (!showAllAvailableUnits && time > TimeSpan.FromHours(1.0))
			{
				num2++;
			}
			string text4 = text;
			object obj2 = num2;
			string text5;
			if (abbreviated)
			{
				text5 = " " + Language.GetTextValue("Misc.ShortHours");
			}
			else
			{
				text5 = ((num2 == 1) ? " hour" : " hours");
			}
			text = string.Concat(text4, obj2, text5);
			if (!showAllAvailableUnits)
			{
				return text;
			}
			text += " ";
		}
		if (time.Minutes > 0)
		{
			int num3 = time.Minutes;
			if (!showAllAvailableUnits && time > TimeSpan.FromMinutes(1.0))
			{
				num3++;
			}
			string text6 = text;
			object obj3 = num3;
			string text7;
			if (abbreviated)
			{
				text7 = " " + Language.GetTextValue("Misc.ShortMinutes");
			}
			else
			{
				text7 = ((num3 == 1) ? " minute" : " minutes");
			}
			text = string.Concat(text6, obj3, text7);
			if (!showAllAvailableUnits)
			{
				return text;
			}
			text += " ";
		}
		string text8 = text;
		object obj4 = time.Seconds;
		string text9;
		if (abbreviated)
		{
			text9 = " " + Language.GetTextValue("Misc.ShortSeconds");
		}
		else
		{
			text9 = ((time.Seconds == 1) ? " second" : " seconds");
		}
		return string.Concat(text8, obj4, text9);
	}
}
