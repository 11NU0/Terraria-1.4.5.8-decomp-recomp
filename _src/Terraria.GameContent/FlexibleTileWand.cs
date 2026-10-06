using System.Collections.Generic;
using Terraria.Utilities;

namespace Terraria.GameContent;

public class FlexibleTileWand
{
	private class OptionBucket
	{
		public int ItemTypeToConsume;

		public List<PlacementOption> Options;

		public OptionBucket(int itemTypeToConsume)
		{
			ItemTypeToConsume = itemTypeToConsume;
			Options = new List<PlacementOption>();
		}

		public PlacementOption GetRandomOption(UnifiedRandom random)
		{
			return Options[random.Next(Options.Count)];
		}

		public PlacementOption GetOptionWithCycling(int cycleOffset)
		{
			int count = Options.Count;
			int index = (cycleOffset % count + count) % count;
			return Options[index];
		}
	}

	public class PlacementOption
	{
		public int TileIdToPlace;

		public int TileStyleToPlace;
	}

	public static FlexibleTileWand RubblePlacementSmall = CreateRubblePlacerSmall();

	public static FlexibleTileWand RubblePlacementMedium = CreateRubblePlacerMedium();

	public static FlexibleTileWand RubblePlacementLarge = CreateRubblePlacerLarge();

	public static FlexibleTileWand MiteyTitey = CreateMiteyTitey();

	public static FlexibleTileWand SandCastleBucket = CreateSingleTileWand(169, 552, 0, 1, 2, 3).WithoutAmmoIcon();

	public static FlexibleTileWand GardenGnome = CreateSingleTileWand(4609, 567, 0, 1, 2, 3, 4).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand Coral = CreateSingleTileWand(275, 81, 0, 1, 2, 3, 4, 5).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand Seashell = CreateSingleTileWand(2625, 324, 0, 1, 2).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand Starfish = CreateSingleTileWand(2626, 324, 3, 4, 5).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand LightningWhelkShell = CreateSingleTileWand(4072, 324, 6, 7, 8).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand TulipShell = CreateSingleTileWand(4073, 324, 9, 10, 11).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand JunoniaShell = CreateSingleTileWand(4071, 324, 12, 13, 14).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand JackoLantern = CreateSingleTileWand(1813, 35, 0, 1, 2, 3, 4, 5, 6, 7, 8).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand Catacomb = CreateSingleTileWand(1417, 241, 0, 1, 2, 3, 4, 5, 6, 7, 8).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand Present = CreateSingleTileWand(1869, 36, 0, 1, 2, 3, 4, 5, 6, 7).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand PartyPresent = CreateSingleTileWand(3749, 457, 0, 1, 2, 3, 4).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand Book = CreateSingleTileWand(149, 50, 0, 1, 2, 3, 4).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand LawnFlamingo = CreateSingleTileWand(4420, 545, 0, 1).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand PortableKiln = CreatePortableKiln();

	public static FlexibleTileWand DeadCellsDisplayJar = CreateSingleTileWand(5472, 698, 1, 2, 0).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand Amethyst = ExposedGem(181, 0);

	public static FlexibleTileWand Topaz = ExposedGem(180, 1);

	public static FlexibleTileWand Sapphire = ExposedGem(177, 2);

	public static FlexibleTileWand Emerald = ExposedGem(179, 3);

	public static FlexibleTileWand Ruby = ExposedGem(178, 4);

	public static FlexibleTileWand Diamond = ExposedGem(182, 5);

	public static FlexibleTileWand Amber = ExposedGem(999, 6);

	public static FlexibleTileWand Explosives = CreateSingleTileWand(580, 141, 0, 1).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand CrystalShard = CreateSingleTileWand(502, 129, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17).WithoutAmmoIcon().WithoutAmmoConsumption();

	public static FlexibleTileWand GiantTiki = CreateSingleTileWand(6147, 753, 0, 1, 2).WithoutAmmoIcon().WithoutAmmoConsumption();

	private UnifiedRandom _random = new UnifiedRandom();

	private Dictionary<int, OptionBucket> _options = new Dictionary<int, OptionBucket>();

	public bool ConsumesAmmoItem = true;

	public bool ShowsHoverAmmoIcon = true;

	public bool CanConsumeFavorites = true;

	public static FlexibleTileWand ExposedGem(int itemId, int style)
	{
		return CreateSingleTileWand(itemId, 178, style, style + 7, style + 14).WithoutAmmoIcon().WithoutAmmoConsumption();
	}

	public FlexibleTileWand WithoutAmmoIcon()
	{
		ShowsHoverAmmoIcon = false;
		return this;
	}

	public FlexibleTileWand WithoutAmmoConsumption()
	{
		ConsumesAmmoItem = false;
		return this;
	}

	public FlexibleTileWand WithConsumingFavorites()
	{
		CanConsumeFavorites = true;
		return this;
	}

	public void AddVariation(int itemType, int tileIdToPlace, int tileStyleToPlace)
	{
		if (!_options.TryGetValue(itemType, out var value))
		{
			OptionBucket optionBucket = (_options[itemType] = new OptionBucket(itemType));
			value = optionBucket;
		}
		value.Options.Add(new PlacementOption
		{
			TileIdToPlace = tileIdToPlace,
			TileStyleToPlace = tileStyleToPlace
		});
	}

	public void AddVariations(int itemType, int tileIdToPlace, params int[] stylesToPlace)
	{
		foreach (int tileStyleToPlace in stylesToPlace)
		{
			AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
		}
	}

	public void AddVariationsWithOffset(int itemType, int tileIdToPlace, int offset, params int[] stylesToPlace)
	{
		for (int i = 0; i < stylesToPlace.Length; i++)
		{
			int tileStyleToPlace = offset + stylesToPlace[i];
			AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
		}
	}

	public void AddVariations_ByRow(int itemType, int tileIdToPlace, int variationsPerRow, params int[] rows)
	{
		for (int i = 0; i < rows.Length; i++)
		{
			for (int j = 0; j < variationsPerRow; j++)
			{
				int tileStyleToPlace = rows[i] * variationsPerRow + j;
				AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
			}
		}
	}

	public bool TryGetPlacementOption(Player player, int randomSeed, int selectCycleOffset, out PlacementOption option, out Item itemToConsume)
	{
		option = null;
		itemToConsume = null;
		Item[] inventory = player.inventory;
		int num = 1;
		for (int i = 0; i < 58 + num; i++)
		{
			if (i < 50 || i >= 54)
			{
				Item item = inventory[i];
				if (!item.IsAir && (CanConsumeFavorites || !item.favorited) && _options.TryGetValue(item.type, out var value))
				{
					_random.SetSeed(randomSeed);
					option = value.GetOptionWithCycling(selectCycleOffset);
					itemToConsume = item;
					return true;
				}
			}
		}
		return false;
	}

	public static FlexibleTileWand CreateRubblePlacerLarge()
	{
		FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
		int tileIdToPlace = 647;
		flexibleTileWand.AddVariations(154, tileIdToPlace, 0, 1, 2, 3, 4, 5, 6);
		flexibleTileWand.AddVariations(3, tileIdToPlace, 7, 8, 9, 10, 11, 12, 13, 14, 15);
		flexibleTileWand.AddVariations(71, tileIdToPlace, 16, 17);
		flexibleTileWand.AddVariations(72, tileIdToPlace, 18, 19);
		flexibleTileWand.AddVariations(73, tileIdToPlace, 20, 21);
		flexibleTileWand.AddVariations(9, tileIdToPlace, 22, 23, 24, 25);
		flexibleTileWand.AddVariations(593, tileIdToPlace, 26, 27, 28, 29, 30, 31);
		flexibleTileWand.AddVariations(183, tileIdToPlace, 32, 33, 34);
		tileIdToPlace = 648;
		flexibleTileWand.AddVariations(195, tileIdToPlace, 0, 1, 2);
		flexibleTileWand.AddVariations(195, tileIdToPlace, 3, 4, 5);
		flexibleTileWand.AddVariations(174, tileIdToPlace, 6, 7, 8);
		flexibleTileWand.AddVariation(4144, 706, 0);
		flexibleTileWand.AddVariations(150, tileIdToPlace, 9, 10, 11, 12, 13);
		flexibleTileWand.AddVariations(3, tileIdToPlace, 14, 15, 16);
		flexibleTileWand.AddVariations(989, tileIdToPlace, 17);
		flexibleTileWand.AddVariations(1101, tileIdToPlace, 18, 19, 20);
		flexibleTileWand.AddVariations(9, tileIdToPlace, 21, 22);
		flexibleTileWand.AddVariations(9, tileIdToPlace, 23, 24, 25, 26, 27, 28);
		flexibleTileWand.AddVariations(3271, tileIdToPlace, 29, 30, 31, 32, 33, 34);
		flexibleTileWand.AddVariations(3086, tileIdToPlace, 35, 36, 37, 38, 39, 40);
		flexibleTileWand.AddVariations(3081, tileIdToPlace, 41, 42, 43, 44, 45, 46);
		flexibleTileWand.AddVariations(62, tileIdToPlace, 47, 48, 49);
		flexibleTileWand.AddVariations(62, tileIdToPlace, 50, 51);
		flexibleTileWand.AddVariations(154, tileIdToPlace, 52, 53, 54);
		tileIdToPlace = 651;
		flexibleTileWand.AddVariations(195, tileIdToPlace, 0, 1, 2);
		flexibleTileWand.AddVariations(62, tileIdToPlace, 3, 4, 5);
		flexibleTileWand.AddVariations(331, tileIdToPlace, 6, 7, 8);
		flexibleTileWand.AddVariation(501, 704, 0);
		tileIdToPlace = 705;
		flexibleTileWand.AddVariations(276, tileIdToPlace, 0, 1, 2, 3, 4, 5, 6, 7, 8);
		flexibleTileWand.AddVariations(369, tileIdToPlace, 9, 10, 11, 12, 13, 14, 15, 16, 17);
		flexibleTileWand.AddVariations(2171, tileIdToPlace, 18, 19, 20, 21, 22, 23, 24, 25, 26);
		flexibleTileWand.AddVariations(59, tileIdToPlace, 27, 28, 29, 30, 31, 32, 33, 34, 35);
		return flexibleTileWand;
	}

	public static FlexibleTileWand CreateRubblePlacerMedium()
	{
		FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
		ushort tileIdToPlace = 652;
		flexibleTileWand.AddVariations(195, tileIdToPlace, 0, 1, 2);
		flexibleTileWand.AddVariations(62, tileIdToPlace, 3, 4, 5);
		flexibleTileWand.AddVariations(331, tileIdToPlace, 6, 7, 8, 9, 10, 11);
		tileIdToPlace = 649;
		flexibleTileWand.AddVariations(3, tileIdToPlace, 0, 1, 2, 3, 4, 5);
		flexibleTileWand.AddVariations(154, tileIdToPlace, 6, 7, 8, 9, 10);
		flexibleTileWand.AddVariations(154, tileIdToPlace, 11, 12, 13, 14, 15);
		flexibleTileWand.AddVariations(71, tileIdToPlace, 16);
		flexibleTileWand.AddVariations(72, tileIdToPlace, 17);
		flexibleTileWand.AddVariations(73, tileIdToPlace, 18);
		flexibleTileWand.AddVariations(181, tileIdToPlace, 19);
		flexibleTileWand.AddVariations(180, tileIdToPlace, 20);
		flexibleTileWand.AddVariations(177, tileIdToPlace, 21);
		flexibleTileWand.AddVariations(179, tileIdToPlace, 22);
		flexibleTileWand.AddVariations(178, tileIdToPlace, 23);
		flexibleTileWand.AddVariations(182, tileIdToPlace, 24);
		flexibleTileWand.AddVariations(593, tileIdToPlace, 25, 26, 27, 28, 29, 30);
		flexibleTileWand.AddVariations(9, tileIdToPlace, 31, 32, 33);
		flexibleTileWand.AddVariations(150, tileIdToPlace, 34, 35, 36, 37);
		flexibleTileWand.AddVariations(3, tileIdToPlace, 38, 39, 40);
		flexibleTileWand.AddVariations(3271, tileIdToPlace, 41, 42, 43, 44, 45, 46);
		flexibleTileWand.AddVariations(3086, tileIdToPlace, 47, 48, 49, 50, 51, 52);
		flexibleTileWand.AddVariations(3081, tileIdToPlace, 53, 54, 55, 56, 57, 58);
		flexibleTileWand.AddVariations(62, tileIdToPlace, 59, 60, 61);
		flexibleTileWand.AddVariations(169, tileIdToPlace, 62, 63, 65, 66, 67);
		flexibleTileWand.AddVariations(276, tileIdToPlace, 64);
		flexibleTileWand.AddVariations(1291, 702, 0, 1, 2);
		flexibleTileWand.AddVariations(5667, 751, default(int));
		return flexibleTileWand;
	}

	public static FlexibleTileWand CreateRubblePlacerSmall()
	{
		FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
		ushort tileIdToPlace = 650;
		flexibleTileWand.AddVariations(3, tileIdToPlace, 0, 1, 2, 3, 4, 5);
		flexibleTileWand.AddVariations(2, tileIdToPlace, 6, 7, 8, 9, 10, 11);
		flexibleTileWand.AddVariations(154, tileIdToPlace, 12, 13, 14, 15, 16, 17, 18, 19);
		flexibleTileWand.AddVariations(154, tileIdToPlace, 20, 21, 22, 23, 24, 25, 26, 27);
		flexibleTileWand.AddVariations(9, tileIdToPlace, 28, 29, 30, 31, 32);
		flexibleTileWand.AddVariations(9, tileIdToPlace, 33, 34, 35);
		flexibleTileWand.AddVariations(593, tileIdToPlace, 36, 37, 38, 39, 40, 41);
		flexibleTileWand.AddVariations(664, tileIdToPlace, 42, 43, 44, 45, 46, 47);
		flexibleTileWand.AddVariations(150, tileIdToPlace, 48, 49, 50, 51, 52, 53);
		flexibleTileWand.AddVariations(3271, tileIdToPlace, 54, 55, 56, 57, 58, 59);
		flexibleTileWand.AddVariations(3086, tileIdToPlace, 60, 61, 62, 63, 64, 65);
		flexibleTileWand.AddVariations(3081, tileIdToPlace, 66, 67, 68, 69, 70, 71);
		flexibleTileWand.AddVariations(62, tileIdToPlace, 72);
		flexibleTileWand.AddVariations(169, tileIdToPlace, 73, 74, 76, 78, 79, 80, 81);
		flexibleTileWand.AddVariations(276, tileIdToPlace, 75, 77);
		flexibleTileWand.AddVariation(5114, 700, 0);
		flexibleTileWand.AddVariation(5333, 701, 0);
		flexibleTileWand.AddVariations(208, 703, 6, 7);
		flexibleTileWand.AddVariations(331, 703, 8);
		flexibleTileWand.AddVariations(223, 703, 9);
		flexibleTileWand.AddVariation(165, 707, 5);
		return flexibleTileWand;
	}

	public static FlexibleTileWand CreateSingleTileWand(int itemIdToConsume, int TileTypeToplace, params int[] stylesToPlace)
	{
		FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
		flexibleTileWand.AddVariations(itemIdToConsume, TileTypeToplace, stylesToPlace);
		return flexibleTileWand;
	}

	public static FlexibleTileWand CreateMiteyTitey()
	{
		FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
		ushort tileIdToPlace = 693;
		flexibleTileWand.AddVariations(664, tileIdToPlace, 0, 1, 2);
		flexibleTileWand.AddVariations(3, tileIdToPlace, 3, 4, 5);
		flexibleTileWand.AddVariations(1124, tileIdToPlace, 9, 10, 11);
		flexibleTileWand.AddVariations(409, tileIdToPlace, 12, 13, 14);
		flexibleTileWand.AddVariations(61, tileIdToPlace, 15, 16, 17);
		flexibleTileWand.AddVariations(836, tileIdToPlace, 18, 19, 20);
		flexibleTileWand.AddVariations(3271, tileIdToPlace, 21, 22, 23);
		flexibleTileWand.AddVariations(3086, tileIdToPlace, 24, 25, 26);
		flexibleTileWand.AddVariations(3081, tileIdToPlace, 27, 28, 29);
		flexibleTileWand.AddVariations(834, tileIdToPlace, 30, 31, 32);
		flexibleTileWand.AddVariations(833, tileIdToPlace, 33, 34, 35);
		flexibleTileWand.AddVariations(835, tileIdToPlace, 36, 37, 38);
		int offset = 39;
		flexibleTileWand.AddVariationsWithOffset(3, tileIdToPlace, offset, 3, 4, 5);
		flexibleTileWand.AddVariationsWithOffset(1124, tileIdToPlace, offset, 9, 10, 11);
		flexibleTileWand.AddVariationsWithOffset(409, tileIdToPlace, offset, 12, 13, 14);
		flexibleTileWand.AddVariationsWithOffset(61, tileIdToPlace, offset, 15, 16, 17);
		flexibleTileWand.AddVariationsWithOffset(836, tileIdToPlace, offset, 18, 19, 20);
		flexibleTileWand.AddVariationsWithOffset(3271, tileIdToPlace, offset, 21, 22, 23);
		flexibleTileWand.AddVariationsWithOffset(3086, tileIdToPlace, offset, 24, 25, 26);
		flexibleTileWand.AddVariationsWithOffset(3081, tileIdToPlace, offset, 27, 28, 29);
		tileIdToPlace = 694;
		flexibleTileWand.AddVariations(664, tileIdToPlace, 0, 1, 2);
		flexibleTileWand.AddVariations(3, tileIdToPlace, 3, 4, 5);
		flexibleTileWand.AddVariations(150, tileIdToPlace, 6, 7, 8);
		flexibleTileWand.AddVariations(409, tileIdToPlace, 12, 13, 14);
		flexibleTileWand.AddVariations(61, tileIdToPlace, 15, 16, 17);
		flexibleTileWand.AddVariations(836, tileIdToPlace, 18, 19, 20);
		flexibleTileWand.AddVariations(3271, tileIdToPlace, 21, 22, 23);
		flexibleTileWand.AddVariations(3086, tileIdToPlace, 24, 25, 26);
		flexibleTileWand.AddVariations(3081, tileIdToPlace, 27, 28, 29);
		flexibleTileWand.AddVariations(834, tileIdToPlace, 30, 31, 32);
		flexibleTileWand.AddVariations(833, tileIdToPlace, 33, 34, 35);
		flexibleTileWand.AddVariations(835, tileIdToPlace, 36, 37, 38);
		flexibleTileWand.AddVariationsWithOffset(3, tileIdToPlace, offset, 3, 4, 5);
		flexibleTileWand.AddVariationsWithOffset(409, tileIdToPlace, offset, 12, 13, 14);
		flexibleTileWand.AddVariationsWithOffset(61, tileIdToPlace, offset, 15, 16, 17);
		flexibleTileWand.AddVariationsWithOffset(836, tileIdToPlace, offset, 18, 19, 20);
		flexibleTileWand.AddVariationsWithOffset(3271, tileIdToPlace, offset, 21, 22, 23);
		flexibleTileWand.AddVariationsWithOffset(3086, tileIdToPlace, offset, 24, 25, 26);
		flexibleTileWand.AddVariationsWithOffset(3081, tileIdToPlace, offset, 27, 28, 29);
		return flexibleTileWand;
	}

	public static FlexibleTileWand CreatePortableKiln()
	{
		FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
		int variationsPerRow = 3;
		int tileIdToPlace = 653;
		flexibleTileWand.AddVariations_ByRow(133, tileIdToPlace, variationsPerRow, 0, 1, 2, 3);
		flexibleTileWand.AddVariations_ByRow(664, tileIdToPlace, variationsPerRow, 4, 5, 6);
		flexibleTileWand.AddVariations_ByRow(4564, tileIdToPlace, variationsPerRow, 7, 8, 9);
		flexibleTileWand.AddVariations_ByRow(154, tileIdToPlace, variationsPerRow, 10, 11, 12);
		flexibleTileWand.AddVariations_ByRow(173, tileIdToPlace, variationsPerRow, 13, 14, 15);
		flexibleTileWand.AddVariations_ByRow(61, tileIdToPlace, variationsPerRow, 16, 17, 18);
		flexibleTileWand.AddVariations_ByRow(150, tileIdToPlace, variationsPerRow, 19, 20, 21);
		flexibleTileWand.AddVariations_ByRow(836, tileIdToPlace, variationsPerRow, 22, 23, 24);
		flexibleTileWand.AddVariations_ByRow(3272, tileIdToPlace, variationsPerRow, 25, 26, 27);
		flexibleTileWand.AddVariations_ByRow(1101, tileIdToPlace, variationsPerRow, 28, 29, 30);
		flexibleTileWand.AddVariations_ByRow(3081, tileIdToPlace, variationsPerRow, 31, 32, 33);
		flexibleTileWand.AddVariations_ByRow(3271, tileIdToPlace, variationsPerRow, 34, 35, 36);
		return flexibleTileWand;
	}
}
