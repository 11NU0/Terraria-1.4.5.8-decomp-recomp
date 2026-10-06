using Terraria.DataStructures;
using Terraria.GameContent.LeashedEntities;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities;

public class TECritterAnchor : TELeashedEntityAnchorWithItem
{
	private static byte _myEntityID;

	public static LeashedCritter[] CritterPrototypes;

	public TECritterAnchor()
	{
		type = _myEntityID;
	}

	public override void RegisterTileEntityID(int assignedID)
	{
		type = (_myEntityID = (byte)assignedID);
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile.active())
		{
			return tile.type == 724;
		}
		return false;
	}

	public override TileEntity GenerateInstance()
	{
		return new TECritterAnchor();
	}

	public static void Kill(int x, int y)
	{
		TileEntity.Kill(x, y, _myEntityID);
	}

	public static int Hook_AfterPlacement(int x, int y, int type, int style, int direction, int alternate)
	{
		return TELeashedEntityAnchorWithItem.PlaceFromPlayerPlacementHook(x, y, _myEntityID);
	}

	public override bool FitsItem(int itemType)
	{
		return ContentSamples.ItemsByType[itemType].makeNPC > 0;
	}

	public override LeashedEntity CreateLeashedEntity()
	{
		if (itemType <= 0)
		{
			return null;
		}
		LeashedCritter leashedCritter = (LeashedCritter)GetLeashedCritterPrototype(itemType).NewInstance();
		leashedCritter.SetDefaults(itemType);
		return leashedCritter;
	}

	static TECritterAnchor()
	{
		CritterPrototypes = NPCID.Sets.Factory.CreateCustomSet((LeashedCritter)WalkerLeashedCritter.Prototype, new object[0]);
		SetPrototypeCollection(FlyerLeashedCritter.Prototype, 444, 653, 661);
		SetPrototypeCollection(NormalButterflyLeashedCritter.Prototype, 356);
		SetPrototypeCollection(EmpressButterflyLeashedCritter.Prototype, 661);
		SetPrototypeCollection(HellButterflyLeashedCritter.Prototype, 653);
		SetPrototypeCollection(FireflyLeashedCritter.Prototype, 355, 358, 654);
		SetPrototypeCollection(ShimmerFlyLeashedCritter.Prototype, 677);
		SetPrototypeCollection(DragonflyLeashedCritter.Prototype, 595, 596, 601, 597, 598, 599, 600);
		SetPrototypeCollection(CrawlingFlyLeashedCritter.Prototype, 604, 605, 669);
		SetPrototypeCollection(FairyLeashedCritter.Prototype, 585, 584, 583);
		SetPrototypeCollection(AquaticWalkerLeashedCritter.Prototype, 361, 445, 148, 149);
		SetPrototypeCollection(CrawlerLeashedCritter.Prototype, 357, 448, 484, 485, 486, 487, 606);
		SetPrototypeCollection(SnailLeashedCritter.Prototype, 359, 360, 655);
		SetPrototypeCollection(TurtleLeashedCritter.Prototype, 616, 617);
		SetPrototypeCollection(RunnerLeashedCritter.Prototype, 300, 447, 610);
		SetPrototypeCollection(BirdLeashedCritter.Prototype, 74, 297, 298, 442, 611, 671, 672, 673, 675, 674);
		SetPrototypeCollection(WaterfowlLeashedCritter.Prototype, 362, 364, 602, 608);
		SetPrototypeCollection(FishLeashedCritter.Prototype, 55, 592, 607, 626, 627, 688);
		SetPrototypeCollection(JumperLeashedCritter.Prototype, 377, 446);
		SetPrototypeCollection(WaterStriderLeashedCritter.Prototype, 612, 613);
	}

	public static void SetPrototypeCollection(LeashedCritter instance, params int[] targetIds)
	{
		foreach (int num in targetIds)
		{
			CritterPrototypes[num] = instance;
		}
	}

	public static LeashedCritter GetLeashedCritterPrototype(int itemType)
	{
		return CritterPrototypes[ContentSamples.ItemsByType[itemType].makeNPC];
	}
}
