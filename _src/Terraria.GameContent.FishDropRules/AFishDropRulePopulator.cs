using System.Linq;

namespace Terraria.GameContent.FishDropRules;

public abstract class AFishDropRulePopulator
{
	private class DelegateFishingCondition : AFishingCondition
	{
		public delegate bool MatchCondition(FishingContext context);

		private MatchCondition _condition;

		public DelegateFishingCondition(MatchCondition innerCondition)
		{
			_condition = innerCondition;
		}

		public override bool Matches(FishingContext context)
		{
			return _condition(context);
		}
	}

	private class DelegateFishingRarityCondition : FishRarityCondition
	{
		public delegate bool MatchCondition(FishingContext context);

		private MatchCondition _condition;

		public DelegateFishingRarityCondition(MatchCondition innerCondition)
		{
			_condition = innerCondition;
		}

		public override bool Matches(FishingContext context)
		{
			return _condition(context);
		}
	}

	protected class Rarity
	{
		public static FishRarityCondition Any = new DelegateFishingRarityCondition((FishingContext context) => true)
		{
			HackedIsAny = true,
			FrequencyOfAppearanceForVisuals = 1f
		};

		public static FishRarityCondition Legendary = new DelegateFishingRarityCondition((FishingContext context) => context.Fisher.legendary)
		{
			FrequencyOfAppearanceForVisuals = 0.1f
		};

		public static FishRarityCondition VeryRare = new DelegateFishingRarityCondition((FishingContext context) => context.Fisher.veryrare)
		{
			FrequencyOfAppearanceForVisuals = 0.25f
		};

		public static FishRarityCondition Rare = new DelegateFishingRarityCondition((FishingContext context) => context.Fisher.rare)
		{
			FrequencyOfAppearanceForVisuals = 0.4f
		};

		public static FishRarityCondition Uncommon = new DelegateFishingRarityCondition((FishingContext context) => context.Fisher.uncommon)
		{
			FrequencyOfAppearanceForVisuals = 0.8f
		};

		public static FishRarityCondition Common = new DelegateFishingRarityCondition((FishingContext context) => context.Fisher.common)
		{
			FrequencyOfAppearanceForVisuals = 1f
		};

		public static FishRarityCondition BombRarityOfNotLegendaryAndNotVeryRareAndUncommon = new DelegateFishingRarityCondition((FishingContext context) => !context.Fisher.legendary && !context.Fisher.veryrare && context.Fisher.uncommon)
		{
			FrequencyOfAppearanceForVisuals = 0.6f
		};

		public static FishRarityCondition UncommonOrCommon = new DelegateFishingRarityCondition((FishingContext context) => context.Fisher.uncommon || context.Fisher.common)
		{
			FrequencyOfAppearanceForVisuals = 1f
		};
	}

	private FishDropRuleList _list;

	protected AFishingCondition HardMode = new DelegateFishingCondition((FishingContext context) => IsHardmode(state: true));

	protected AFishingCondition EarlyMode = new DelegateFishingCondition((FishingContext context) => IsHardmode(state: false));

	protected AFishingCondition InLava = new DelegateFishingCondition((FishingContext context) => context.Fisher.inLava);

	protected AFishingCondition InHoney = new DelegateFishingCondition((FishingContext context) => context.Fisher.inHoney);

	protected AFishingCondition Junk = new DelegateFishingCondition((FishingContext context) => context.Fisher.junk);

	protected AFishingCondition Crate = new DelegateFishingCondition((FishingContext context) => context.Fisher.crate);

	protected AFishingCondition AnyEnemies = new DelegateFishingCondition((FishingContext context) => context.Fisher.rolledEnemySpawn > 0);

	protected AFishingCondition CanFishInLava = new DelegateFishingCondition((FishingContext context) => context.Fisher.CanFishInLava);

	protected AFishingCondition Dungeon = new DelegateFishingCondition((FishingContext context) => context.Player.ZoneDungeon && NPC.downedBoss3);

	protected AFishingCondition Beach = new DelegateFishingCondition((FishingContext context) => context.Player.ZoneBeach);

	protected AFishingCondition Hallow = new DelegateFishingCondition((FishingContext context) => context.Player.ZoneHallow);

	protected AFishingCondition GlowingMushrooms = new DelegateFishingCondition((FishingContext context) => context.Player.ZoneGlowshroom);

	protected AFishingCondition TrueDesert = new DelegateFishingCondition((FishingContext context) => context.Player.ZoneDesert);

	protected AFishingCondition TrueSnow = new DelegateFishingCondition((FishingContext context) => context.Player.ZoneSnow);

	protected AFishingCondition Remix = new DelegateFishingCondition((FishingContext context) => Main.remixWorld);

	protected AFishingCondition Height1 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 1);

	protected AFishingCondition Height1And2 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 1 || context.Fisher.heightLevel == 2);

	protected AFishingCondition HeightAbove1 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel > 1);

	protected AFishingCondition HeightAboveAnd1 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel >= 1);

	protected AFishingCondition HeightUnder2 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel < 2);

	protected AFishingCondition HeightAbove2 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel > 2);

	protected AFishingCondition Height0 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 0);

	protected AFishingCondition Height2 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 2);

	protected AFishingCondition Height3 = new DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 3);

	protected AFishingCondition UnderRockLayer = new DelegateFishingCondition((FishingContext context) => (double)context.Fisher.Y >= Main.rockLayer);

	protected AFishingCondition Corruption = new DelegateFishingCondition((FishingContext context) => context.RolledCorruption);

	protected AFishingCondition Crimson = new DelegateFishingCondition((FishingContext context) => context.RolledCrimson);

	protected AFishingCondition Jungle = new DelegateFishingCondition((FishingContext context) => context.RolledJungle);

	protected AFishingCondition Snow = new DelegateFishingCondition((FishingContext context) => context.RolledSnow);

	protected AFishingCondition Desert = new DelegateFishingCondition((FishingContext context) => context.RolledDesert);

	protected AFishingCondition RolledHallowDesert = new DelegateFishingCondition((FishingContext context) => context.RolledInfectedDesert && context.Player.ZoneHallow);

	protected AFishingCondition OriginalOcean = new DelegateFishingCondition((FishingContext context) => IsOriginalOcean(context));

	protected AFishingCondition RemixOcean = new DelegateFishingCondition((FishingContext context) => context.RolledRemixOcean);

	protected AFishingCondition Ocean = new DelegateFishingCondition((FishingContext context) => context.RolledRemixOcean || IsOriginalOcean(context));

	protected AFishingCondition Water1000 = new DelegateFishingCondition((FishingContext context) => context.Fisher.waterTilesCount > 1000);

	protected AFishingCondition BloodMoon = new DelegateFishingCondition((FishingContext context) => Main.bloodMoon);

	protected AFishingCondition DidNotUseCombatBook = new DelegateFishingCondition((FishingContext context) => !NPC.combatBookWasUsed);

	public AFishDropRulePopulator(FishDropRuleList list)
	{
		_list = list;
	}

	protected void Add(FishRarityCondition tier, int chanceNominator, int chanceDenominator, int[] itemTypes, params AFishingCondition[] conditions)
	{
		FishDropRule rule = new FishDropRule
		{
			PossibleItems = itemTypes,
			ChanceNumerator = chanceNominator,
			ChanceDenominator = chanceDenominator,
			Rarity = tier,
			Conditions = conditions
		};
		_list.Add(rule);
	}

	protected void Add(FishRarityCondition tier, int chanceNominator, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
	{
		Add(tier, chanceNominator, chanceDenominator, Group(itemType), conditions);
	}

	protected void Add(FishRarityCondition tier, int chanceDenominator, int[] itemTypes, params AFishingCondition[] conditions)
	{
		Add(tier, 1, chanceDenominator, itemTypes, conditions);
	}

	protected void Add(FishRarityCondition tier, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
	{
		Add(tier, 1, chanceDenominator, Group(itemType), conditions);
	}

	protected void AddQuestFish(FishRarityCondition tier, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
	{
		FishingConditions.QuestFishCondition questFishCondition = new FishingConditions.QuestFishCondition
		{
			CheckedType = itemType
		};
		Add(tier, 1, chanceDenominator, Group(itemType), Join(conditions, questFishCondition));
	}

	protected void AddQuestFishForRemix(FishRarityCondition tier, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
	{
		FishingConditions.QuestFishConditionRemix questFishConditionRemix = new FishingConditions.QuestFishConditionRemix
		{
			CheckedType = itemType
		};
		Add(tier, 1, chanceDenominator, Group(itemType), Join(conditions, questFishConditionRemix));
	}

	protected void AddWithHardmode(FishRarityCondition tier, int chanceDenominator, int itemTypeEarly, int itemTypeHard, params AFishingCondition[] conditions)
	{
		FishDropRule fishDropRule = new FishDropRule();
		fishDropRule.PossibleItems = new int[1] { itemTypeEarly };
		fishDropRule.ChanceNumerator = 1;
		fishDropRule.ChanceDenominator = chanceDenominator;
		fishDropRule.Rarity = tier;
		fishDropRule.Conditions = Join(conditions, EarlyMode);
		FishDropRule rule = fishDropRule;
		_list.Add(rule);
		fishDropRule = new FishDropRule();
		fishDropRule.PossibleItems = new int[1] { itemTypeHard };
		fishDropRule.ChanceNumerator = 1;
		fishDropRule.ChanceDenominator = chanceDenominator;
		fishDropRule.Rarity = tier;
		fishDropRule.Conditions = Join(conditions, HardMode);
		FishDropRule rule2 = fishDropRule;
		_list.Add(rule2);
	}

	protected void AddStopper(AFishingCondition condition)
	{
		Add(Rarity.Any, 1, new int[0], condition);
	}

	public int[] Group(params int[] itemTypes)
	{
		return itemTypes;
	}

	protected AFishingCondition[] Join(AFishingCondition[] original, params AFishingCondition[] additions)
	{
		return original.Concat(additions).ToArray();
	}

	protected AFishingCondition[] Join(params AFishingCondition[] additions)
	{
		return additions;
	}

	private static bool IsHardmode(bool state)
	{
		return Main.hardMode == state;
	}

	private static bool IsOriginalOcean(FishingContext context)
	{
		if (context.Fisher.heightLevel <= 1 && (context.Fisher.X < 380 || context.Fisher.X > Main.maxTilesX - 380))
		{
			return context.Fisher.waterTilesCount > 1000;
		}
		return false;
	}
}
