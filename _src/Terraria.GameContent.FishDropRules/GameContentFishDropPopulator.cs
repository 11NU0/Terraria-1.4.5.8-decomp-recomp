namespace Terraria.GameContent.FishDropRules;

public class GameContentFishDropPopulator : AFishDropRulePopulator
{
	public GameContentFishDropPopulator(FishDropRuleList list)
		: base(list)
	{
	}

	public void Populate()
	{
		AddStopper(AnyEnemies);
		LavaDrops();
		HoneyDrops();
		JunkDrops();
		CrateDrops();
		RareDrops();
		RemixDrops();
		DungeonDrops();
		CorruptionDrops();
		CrimsonDrops();
		HallowedDrops();
		GlowingMushroomsDrops();
		SnowDrops();
		JungleDrops();
		OceanDrops();
		DesertDrops();
		FloatingIslandDrops();
		SurfaceDrops();
	}

	private void RemixDrops()
	{
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2461);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2458);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2459);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2479);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2456);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2474);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2478);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2450);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2464);
		AddQuestFishForRemix(Rarity.Uncommon, 1, 2469);
	}

	private void SurfaceDrops()
	{
		AddQuestFish(Rarity.Uncommon, 1, 2455, Height1And2);
		AddQuestFish(Rarity.Uncommon, 1, 2479, Height1);
		AddQuestFish(Rarity.Uncommon, 1, 2456, Height1);
		AddQuestFish(Rarity.Uncommon, 1, 2474, Height1);
		Add(Rarity.Rare, 10, 2437, HeightAbove1, HardMode);
		Add(Rarity.Rare, 9, 2436, HeightAbove1, HardMode);
		Add(Rarity.Rare, 5, 2436, HeightAbove1, EarlyMode);
		Add(Rarity.Legendary, 2, 3, 2308, HeightAbove1);
		Add(Rarity.VeryRare, 2, 2320, HeightAbove1);
		Add(Rarity.Rare, 1, 2321, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2478, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2450, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2464, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2469, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2462, HeightAbove2);
		AddQuestFish(Rarity.Uncommon, 1, 2482, HeightAbove2);
		AddQuestFish(Rarity.Uncommon, 1, 2472, HeightAbove2);
		AddQuestFish(Rarity.Uncommon, 1, 2460, HeightAbove2);
		Add(Rarity.Uncommon, 3, 4, 2303, HeightAbove1);
		Add(Rarity.UncommonOrCommon, 4, Group(2303, 2309, 2309, 2309), HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2487);
		Add(Rarity.Common, 1, 2298, Water1000);
		Add(Rarity.Any, 1, 2290);
	}

	private void FloatingIslandDrops()
	{
		AddQuestFish(Rarity.Uncommon, 1, 2461, HeightUnder2);
		AddQuestFish(Rarity.Uncommon, 1, 2453, Height0);
		AddQuestFish(Rarity.Uncommon, 1, 2473, Height0);
		AddQuestFish(Rarity.Uncommon, 1, 2476, Height0);
		AddQuestFish(Rarity.Uncommon, 1, 2458, HeightUnder2);
		AddQuestFish(Rarity.Uncommon, 1, 2459, HeightUnder2);
		Add(Rarity.Uncommon, 1, 2304, Height0);
	}

	private void DesertDrops()
	{
		AFishingCondition desert = Desert;
		Add(Rarity.Legendary, 3, 5490, desert);
		AddQuestFish(Rarity.Uncommon, 1, 4393, desert);
		AddQuestFish(Rarity.Uncommon, 1, 4394, desert);
		Add(Rarity.Uncommon, 1, 4410, desert);
		Add(Rarity.Any, 3, 4402, desert);
		Add(Rarity.Any, 1, 4401, desert);
	}

	private void OceanDrops()
	{
		AFishingCondition ocean = Ocean;
		Add(Rarity.VeryRare, 2, 2341, ocean);
		Add(Rarity.VeryRare, 1, 2342, ocean);
		Add(Rarity.Rare, 5, 2438, ocean);
		Add(Rarity.Rare, 3, 2332, ocean);
		AddQuestFish(Rarity.Uncommon, 1, 2480, ocean);
		AddQuestFish(Rarity.Uncommon, 1, 2481, ocean);
		Add(Rarity.Uncommon, 1, 2316, ocean);
		Add(Rarity.Common, 2, 2301, ocean);
		Add(Rarity.Common, 1, 2300, ocean);
		Add(Rarity.Any, 1, 2297, ocean);
		AddStopper(ocean);
	}

	private void JungleDrops()
	{
		AFishingCondition jungle = Jungle;
		Add(Rarity.Legendary, 2, 3, 5463, jungle, HardMode);
		Add(Rarity.Legendary, 2, 3, 5634, jungle);
		AddQuestFish(Rarity.Uncommon, 1, 2452, jungle, Height1);
		AddQuestFish(Rarity.Uncommon, 1, 2483, jungle, Height1);
		AddQuestFish(Rarity.Uncommon, 1, 2488, jungle, Height1);
		AddQuestFish(Rarity.Uncommon, 1, 2486, jungle, HeightAboveAnd1);
		Add(Rarity.Uncommon, 1, 2311, jungle, HeightAbove1);
		Add(Rarity.Uncommon, 1, 2313, jungle);
		Add(Rarity.Common, 1, 2302, jungle);
	}

	private void SnowDrops()
	{
		AFishingCondition snow = Snow;
		AddQuestFish(Rarity.Uncommon, 1, 2467, snow, HeightUnder2);
		AddQuestFish(Rarity.Uncommon, 1, 2470, snow, Height1);
		AddQuestFish(Rarity.Uncommon, 1, 2484, snow, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2466, snow, HeightAbove1);
		Add(Rarity.Common, 12, 3197, snow);
		Add(Rarity.Uncommon, 6, 3197, snow);
		Add(Rarity.Uncommon, 1, 2306, snow);
		Add(Rarity.Common, 1, 2299, snow);
		Add(Rarity.Any, 3, 2309, snow, HeightAbove1);
	}

	private void GlowingMushroomsDrops()
	{
		AddQuestFish(Rarity.Uncommon, 1, 2475, GlowingMushrooms);
	}

	private void HallowedDrops()
	{
		AFishingCondition rolledHallowDesert = RolledHallowDesert;
		Add(Rarity.Legendary, 1, 5490, rolledHallowDesert);
		AddQuestFish(Rarity.Uncommon, 1, 4393, rolledHallowDesert);
		AddQuestFish(Rarity.Uncommon, 1, 4394, rolledHallowDesert);
		Add(Rarity.Uncommon, 1, 4410, rolledHallowDesert);
		Add(Rarity.Any, 3, 4402, rolledHallowDesert);
		Add(Rarity.Any, 1, 4401, rolledHallowDesert);
		rolledHallowDesert = Hallow;
		Add(Rarity.Legendary, 2, 3, 2429, TrueSnow, rolledHallowDesert, HardMode, Height3);
		Add(Rarity.Legendary, 2, 3209, rolledHallowDesert, HardMode);
		Add(Rarity.Legendary, 2, 3, 5274, rolledHallowDesert, HardMode);
		Add(Rarity.VeryRare, 1, 2317, rolledHallowDesert, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2465, rolledHallowDesert, HeightAbove1);
		AddQuestFish(Rarity.Uncommon, 1, 2468, rolledHallowDesert, HeightUnder2);
		Add(Rarity.Rare, 1, 2310, rolledHallowDesert);
		AddQuestFish(Rarity.Uncommon, 1, 2471, rolledHallowDesert);
		Add(Rarity.Uncommon, 1, 2307, rolledHallowDesert);
	}

	private void CrimsonDrops()
	{
		AFishingCondition crimson = Crimson;
		Add(Rarity.Legendary, 2, 3, 2429, TrueSnow, crimson, HardMode, Height3);
		Add(Rarity.Legendary, 2, 3211, crimson, HardMode);
		AddQuestFish(Rarity.Uncommon, 1, 2477, crimson);
		AddQuestFish(Rarity.Uncommon, 1, 2463, crimson);
		Add(Rarity.Uncommon, 1, 2319, crimson);
		Add(Rarity.Common, 1, 2305, crimson);
	}

	private void CorruptionDrops()
	{
		AFishingCondition corruption = Corruption;
		Add(Rarity.Legendary, 2, 3, 2429, TrueSnow, corruption, HardMode, Height3);
		Add(Rarity.Legendary, 2, 3210, corruption, HardMode);
		Add(Rarity.Rare, 1, 2330, corruption);
		AddQuestFish(Rarity.Uncommon, 1, 2454, corruption);
		AddQuestFish(Rarity.Uncommon, 1, 2485, corruption);
		AddQuestFish(Rarity.Uncommon, 1, 2457, corruption);
		Add(Rarity.Uncommon, 1, 2318, corruption);
	}

	private void DungeonDrops()
	{
		Add(Rarity.VeryRare, 12, 3000, Dungeon);
		Add(Rarity.VeryRare, 12, 2999, Dungeon);
	}

	private void RareDrops()
	{
		Add(Rarity.Legendary, 2, 4382, BloodMoon, DidNotUseCombatBook);
		Add(Rarity.Legendary, 2, 5240, BloodMoon);
		Add(Rarity.Legendary, 5, 2423);
		Add(Rarity.Legendary, 5, 3225);
		Add(Rarity.Legendary, 10, 2420);
		Add(Rarity.BombRarityOfNotLegendaryAndNotVeryRareAndUncommon, 5, 3196);
	}

	private void CrateDrops()
	{
		AddWithHardmode(Rarity.Rare, 1, 3205, 3984, Crate, Dungeon);
		AddWithHardmode(Rarity.Rare, 1, 5002, 5003, Crate, Beach);
		AddWithHardmode(Rarity.Rare, 1, 3203, 3982, Crate, Corruption);
		AddWithHardmode(Rarity.Rare, 1, 3204, 3983, Crate, Crimson);
		AddWithHardmode(Rarity.Rare, 1, 3207, 3986, Crate, Hallow);
		AddWithHardmode(Rarity.Rare, 1, 3208, 3987, Crate, Jungle);
		AddWithHardmode(Rarity.Rare, 1, 4405, 4406, Crate, Snow);
		AddWithHardmode(Rarity.Rare, 1, 4407, 4408, Crate, TrueDesert);
		AddWithHardmode(Rarity.Rare, 1, 3206, 3985, Crate, Height0);
		AddWithHardmode(Rarity.Rare, 1, 5002, 5003, Crate, Remix, Height1, UnderRockLayer);
		AddWithHardmode(Rarity.Legendary, 1, 2336, 3981, Crate);
		AddWithHardmode(Rarity.VeryRare, 1, 2336, 3981, Crate);
		AddWithHardmode(Rarity.Rare, 1, 2335, 3980, Crate);
		AddWithHardmode(Rarity.Uncommon, 1, 2335, 3980, Crate);
		AddWithHardmode(Rarity.Any, 1, 2334, 3979, Crate);
		AddStopper(Crate);
	}

	private void JunkDrops()
	{
		Add(Rarity.Any, 8, 5275, Junk);
		Add(Rarity.Any, 1, Group(2337, 2338, 2339), Junk);
		AddStopper(Junk);
	}

	private void HoneyDrops()
	{
		Add(Rarity.Rare, 1, 2314, InHoney);
		Add(Rarity.Uncommon, 2, 2314, InHoney);
		AddQuestFish(Rarity.Uncommon, 1, 2451, InHoney);
		AddStopper(InHoney);
	}

	private void LavaDrops()
	{
		AFishingCondition[] array = Join(InLava, CanFishInLava);
		AddWithHardmode(Rarity.Any, 6, 4877, 4878, Join(array, Crate));
		Add(Rarity.Legendary, 3, Group(4819, 4820, 4872, 2331), Join(array, HardMode));
		Add(Rarity.Legendary, 3, Group(4819, 4820, 4872), Join(array, EarlyMode));
		Add(Rarity.VeryRare, 1, 2312, array);
		Add(Rarity.Rare, 1, 2315, array);
		AddStopper(InLava);
	}
}
