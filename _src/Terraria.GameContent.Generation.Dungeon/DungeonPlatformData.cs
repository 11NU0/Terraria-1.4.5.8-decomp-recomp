using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Generation.Dungeon;

public struct DungeonPlatformData
{
	public Point Position;

	public int? OverrideStyle;

	public int OverrideMaxLengthAllowed;

	public int? OverrideHeightFluff;

	public bool InAHallway;

	public bool ForcePlacement;

	public bool SkipOtherPlatformsCheck;

	public bool SkipSpaceCheck;

	public double PlaceBooksChance;

	public bool NoWaterbolt;

	public double PlacePotsChance;

	public double PlaceWaterCandlesChance;

	public double PlacePotionBottlesChance;

	public Func<DungeonData, int, int, bool> canPlaceHereCallback;

	public bool IsAShelf
	{
		get
		{
			if (!(PlaceBooksChance > 0.0) && !(PlacePotsChance > 0.0) && !(PlaceWaterCandlesChance > 0.0))
			{
				return PlacePotionBottlesChance > 0.0;
			}
			return true;
		}
	}
}
