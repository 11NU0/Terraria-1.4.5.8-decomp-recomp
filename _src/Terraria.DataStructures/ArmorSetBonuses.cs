using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Achievements;
using Terraria.ID;

namespace Terraria.DataStructures;

public class ArmorSetBonuses
{
	public static class Benefits
	{
		public static void Tiki(Player player)
		{
			player.maxMinions++;
			player.whipRangeMultiplier += 0.2f;
		}

		public static void Spooky(Player player)
		{
			player.minionDamage += 0.25f;
		}

		public static void Bee(Player player)
		{
			player.minionDamage += 0.1f;
			if (player.itemAnimation > 0 && player.inventory[player.selectedItem].type == 1121)
			{
				AchievementsHelper.HandleSpecialEvent(player, 3);
			}
		}

		public static void Spider(Player player)
		{
			player.minionDamage += 0.12f;
		}

		public static void Solar(Player player)
		{
			player.ApplySetBonus_Solar();
		}

		public static void Vortex(Player player)
		{
			player.setVortex = true;
		}

		public static void Nebula(Player player)
		{
			if (player.nebulaCD > 0)
			{
				player.nebulaCD--;
			}
			player.setNebula = true;
		}

		public static void Stardust(Player player)
		{
			player.ApplySetBonus_Stardust();
		}

		public static void Forbidden(Player player)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			player.setForbidden = true;
			player.UpdateForbiddenSetLock();
			Lighting.AddLight(player.Center, 0.8f, 0.7f, 0.2f);
		}

		public static void SquireTier2(Player player)
		{
			player.setSquireT2 = true;
			player.maxTurrets++;
		}

		public static void ApprenticeTier2(Player player)
		{
			player.setApprenticeT2 = true;
			player.maxTurrets++;
		}

		public static void HuntressTier2(Player player)
		{
			player.setHuntressT2 = true;
			player.maxTurrets++;
		}

		public static void MonkTier2(Player player)
		{
			player.setMonkT2 = true;
			player.maxTurrets++;
		}

		public static void SquireTier3(Player player)
		{
			player.setSquireT3 = true;
			player.setSquireT2 = true;
			player.maxTurrets++;
		}

		public static void ApprenticeTier3(Player player)
		{
			player.setApprenticeT3 = true;
			player.setApprenticeT2 = true;
			player.maxTurrets++;
		}

		public static void HuntressTier3(Player player)
		{
			player.setHuntressT3 = true;
			player.setHuntressT2 = true;
			player.maxTurrets++;
		}

		public static void MonkTier3(Player player)
		{
			player.setMonkT3 = true;
			player.setMonkT2 = true;
			player.maxTurrets++;
		}

		public static void ObsidianOutlaw(Player player)
		{
			player.minionDamage += 0.15f;
			player.whipRangeMultiplier += 0.3f;
			float num = 1.15f;
			float num2 = 1f / num;
			player.whipUseTimeMultiplier *= num2;
		}

		public static void ChlorophyteMelee(Player player)
		{
			player.AddBuff(60, 5);
			player.setChlorophyte = true;
			player.endurance += 0.05f;
		}

		public static void ChlorophyteSummon(Player player)
		{
			player.AddBuff(60, 5);
			player.setChlorophyte = true;
			player.maxMinions += 2;
		}

		public static void Chlorophyte(Player player)
		{
			player.AddBuff(60, 5);
			player.setChlorophyte = true;
		}

		public static void Angler(Player player)
		{
			player.anglerSetSpawnReduction = true;
		}

		public static void Cactus(Player player)
		{
			player.cactusThorns = true;
		}

		public static void Turtle(Player player)
		{
			player.endurance += 0.15f;
			player.thorns = 1f;
			player.turtleThorns = true;
		}

		public static void CobaltCaster(Player player)
		{
			player.manaCost -= 0.14f;
		}

		public static void CobaltMelee(Player player)
		{
			player.meleeSpeed += 0.15f;
		}

		public static void CobaltRanged(Player player)
		{
			player.ammoCost80 = true;
		}

		public static void MythrilCaster(Player player)
		{
			player.manaCost -= 0.17f;
		}

		public static void MythrilMelee(Player player)
		{
			player.meleeCrit += 10;
		}

		public static void MythrilRanged(Player player)
		{
			player.ammoCost80 = true;
		}

		public static void AdamantiteCaster(Player player)
		{
			player.manaCost -= 0.19f;
		}

		public static void AdamantiteMelee(Player player)
		{
			player.meleeSpeed += 0.2f;
			player.moveSpeed += 0.2f;
		}

		public static void AdamantiteRanged(Player player)
		{
			player.ammoCost75 = true;
		}

		public static void Palladium(Player player)
		{
			player.onHitRegen = true;
		}

		public static void Orichalcum(Player player)
		{
			player.onHitPetal = true;
		}

		public static void Titanium(Player player)
		{
			player.onHitTitaniumStorm = true;
		}

		public static void HallowedSummoner(Player player)
		{
			player.maxMinions += 2;
			player.onHitDodge = true;
		}

		public static void Hallowed(Player player)
		{
			player.onHitDodge = true;
		}

		public static void CrystalAssassin(Player player)
		{
			player.rangedDamage += 0.1f;
			player.meleeDamage += 0.1f;
			player.magicDamage += 0.1f;
			player.minionDamage += 0.1f;
			player.rangedCrit += 10;
			player.meleeCrit += 10;
			player.magicCrit += 10;
			player.dashType = 5;
		}

		public static void Crimson(Player player)
		{
			player.crimsonRegen = true;
		}

		public static void SpectreHealing(Player player)
		{
			player.ghostHeal = true;
			player.magicDamage -= 0.4f;
		}

		public static void SpectreDamage(Player player)
		{
			player.ghostHurt = true;
		}

		public static void Meteor(Player player)
		{
			player.spaceGun = true;
		}

		public static void Frost(Player player)
		{
			player.frostBurn = true;
			player.meleeDamage += 0.1f;
			player.rangedDamage += 0.1f;
		}

		public static void Jungle(Player player)
		{
			player.manaCost -= 0.16f;
		}

		public static void Molten(Player player)
		{
			player.meleeDamage += 0.1f;
			player.fireWalk = true;
			if (!player.vampireBurningInSunlight)
			{
				player.buffImmune[24] = true;
			}
		}

		public static void Snow(Player player)
		{
			player.buffImmune[46] = true;
			player.buffImmune[47] = true;
		}

		public static void Mining(Player player)
		{
			player.pickSpeed -= 0.1f;
		}

		public static void Wizard(Player player)
		{
			player.magicCrit += 10;
		}

		public static void MagicHat(Player player)
		{
			player.statManaMax2 += 60;
		}

		public static void ShadowScale(Player player)
		{
			player.shadowArmor = true;
		}

		public static void BeetleDefense(Player player)
		{
			player.ApplySetBonus_BeetleDefense();
		}

		public static void BeetleDamage(Player player)
		{
			player.ApplySetBonus_BeetleDamage();
		}

		public static void Gladiator(Player player)
		{
			player.noKnockback = true;
		}

		public static void Ninja(Player player)
		{
			player.moveSpeed += 0.2f;
		}

		public static void Fossil(Player player)
		{
			player.ammoCost80 = true;
		}

		public static void Necro(Player player)
		{
			player.rangedCrit += 10;
		}

		public static void Pumpkin(Player player)
		{
			player.meleeDamage += 0.1f;
			player.magicDamage += 0.1f;
			player.rangedDamage += 0.1f;
			player.minionDamage += 0.1f;
		}

		public static void Platinum(Player player)
		{
			player.statDefense += 4;
		}

		public static void MetalTier2(Player player)
		{
			player.statDefense += 3;
		}

		public static void MetalTier1(Player player)
		{
			player.statDefense += 2;
		}

		public static void Shroomite(Player player)
		{
			player.shroomiteStealth = true;
		}

		public static void Wood(Player player)
		{
			player.statDefense++;
		}

		public static void AshWood(Player player)
		{
			player.ashWoodBonus = true;
		}
	}

	public static List<ArmorSetBonus> All = new List<ArmorSetBonus>();

	public static ArmorSetBonus[][] SetsContaining;

	public static void Initialize()
	{
		Create(Benefits.Shroomite, "ArmorSetBonus.Shroomite").Set(1548, 1549, 1550).Set(1546, 1549, 1550).Set(1547, 1549, 1550)
			.Add();
		Create(Benefits.Wood, "ArmorSetBonus.Wood").Set(727, 728, 729).Set(733, 734, 735).Set(730, 731, 732)
			.Set(736, 737, 738)
			.Set(924, 925, 926)
			.Set(2509, 2510, 2511)
			.Set(2512, 2513, 2514)
			.Add();
		Add(Benefits.AshWood, "ArmorSetBonus.AshWood", 5279, 5280, 5281);
		Create(Benefits.MetalTier1, "ArmorSetBonus.MetalTier1").Set(89, 80, 76).Set(687, 688, 689).Set(90, 81, 77)
			.Set(954, 81, 77)
			.Add();
		Create(Benefits.MetalTier2, "ArmorSetBonus.MetalTier2").Set(91, 82, 78).Set(92, 83, 79).Set(955, 83, 79)
			.Set(690, 691, 692)
			.Set(693, 694, 695)
			.Add();
		Add(Benefits.Platinum, "ArmorSetBonus.Platinum", 696, 697, 698);
		Add(Benefits.Pumpkin, "ArmorSetBonus.Pumpkin", 1731, 1732, 1733);
		Add(Benefits.Gladiator, "ArmorSetBonus.Gladiator", 3187, 3188, 3189);
		Add(Benefits.Ninja, "ArmorSetBonus.Ninja", 256, 257, 258);
		Add(Benefits.Fossil, "ArmorSetBonus.Fossil", 3374, 3375, 3376);
		Create(Benefits.Necro, "ArmorSetBonus.Bone").Set(151, 152, 153).Set(959, 152, 153).Add();
		Add(Benefits.BeetleDamage, "ArmorSetBonus.BeetleDamage", ArmorSetBonus.PartType.Body, 2199, 2200, 2202);
		Add(Benefits.BeetleDefense, "ArmorSetBonus.BeetleDefense", ArmorSetBonus.PartType.Body, 2199, 2201, 2202);
		Create(Benefits.Wizard, "ArmorSetBonus.Wizard", ArmorSetBonus.PartType.Head).Set(238, 1282, 0).Set(238, 1283, 0).Set(238, 1284, 0)
			.Set(238, 1285, 0)
			.Set(238, 1286, 0)
			.Set(238, 1287, 0)
			.Set(238, 2279, 0)
			.Set(238, 4256, 0)
			.Add();
		Create(Benefits.MagicHat, "ArmorSetBonus.MagicHat", ArmorSetBonus.PartType.Head).Set(2275, 1282, 0).Set(2275, 1283, 0).Set(2275, 1284, 0)
			.Set(2275, 1285, 0)
			.Set(2275, 1286, 0)
			.Set(2275, 1287, 0)
			.Set(2275, 2279, 0)
			.Set(2275, 4256, 0)
			.Add();
		Create(Benefits.ShadowScale, "ArmorSetBonus.ShadowScale").Set(new int[2] { 102, 956 }, new int[2] { 101, 957 }, new int[2] { 100, 958 }).Add();
		Add(Benefits.Crimson, "ArmorSetBonus.Crimson", 792, 793, 794);
		Add(Benefits.SpectreHealing, "ArmorSetBonus.SpectreHealing", ArmorSetBonus.PartType.Head, 1503, 1504, 1505);
		Add(Benefits.SpectreDamage, "ArmorSetBonus.SpectreDamage", ArmorSetBonus.PartType.Head, 2189, 1504, 1505);
		Add(Benefits.Meteor, "ArmorSetBonus.Meteor", 123, 124, 125);
		Add(Benefits.Frost, "ArmorSetBonus.Frost", 684, 685, 686);
		Create(Benefits.Jungle, "ArmorSetBonus.Jungle").Set(new int[2] { 228, 960 }, new int[2] { 229, 961 }, new int[2] { 230, 962 }).Add();
		Add(Benefits.Molten, "ArmorSetBonus.Molten", 231, 232, 233);
		Create(Benefits.Snow, "ArmorSetBonus.Snow").Set(new int[2] { 803, 978 }, new int[2] { 804, 979 }, new int[2] { 805, 980 }).Add();
		Create(Benefits.Mining, "ArmorSetBonus.Mining").Set(new int[3] { 88, 5588, 4008 }, new int[2] { 410, 5589 }, new int[2] { 411, 5590 }).Add();
		Add(Benefits.ChlorophyteMelee, "ArmorSetBonus.ChlorophyteMelee", ArmorSetBonus.PartType.Head, 1001, 1004, 1005);
		Add(Benefits.ChlorophyteSummon, "ArmorSetBonus.ChlorophyteSummon", ArmorSetBonus.PartType.Head, 5524, 1004, 1005);
		Create(Benefits.Chlorophyte, "ArmorSetBonus.Chlorophyte", ArmorSetBonus.PartType.Head).Set(1003, 1004, 1005).Set(1002, 1004, 1005).Add();
		Create(Benefits.Angler, "ArmorSetBonus.Angler").Set(new int[2] { 2367, 5591 }, new int[2] { 2368, 5592 }, new int[2] { 2369, 5593 }).Add();
		Add(Benefits.Cactus, "ArmorSetBonus.Cactus", 894, 895, 896);
		Add(Benefits.Turtle, "ArmorSetBonus.Turtle", 1316, 1317, 1318);
		Add(Benefits.CobaltCaster, "ArmorSetBonus.CobaltCaster", ArmorSetBonus.PartType.Head, 371, 374, 375);
		Add(Benefits.CobaltMelee, "ArmorSetBonus.CobaltMelee", ArmorSetBonus.PartType.Head, 372, 374, 375);
		Add(Benefits.CobaltRanged, "ArmorSetBonus.CobaltRanged", ArmorSetBonus.PartType.Head, 373, 374, 375);
		Add(Benefits.MythrilCaster, "ArmorSetBonus.MythrilCaster", ArmorSetBonus.PartType.Head, 376, 379, 380);
		Add(Benefits.MythrilMelee, "ArmorSetBonus.MythrilMelee", ArmorSetBonus.PartType.Head, 377, 379, 380);
		Add(Benefits.MythrilRanged, "ArmorSetBonus.MythrilRanged", ArmorSetBonus.PartType.Head, 378, 379, 380);
		Add(Benefits.AdamantiteCaster, "ArmorSetBonus.AdamantiteCaster", ArmorSetBonus.PartType.Head, 400, 403, 404);
		Add(Benefits.AdamantiteMelee, "ArmorSetBonus.AdamantiteMelee", ArmorSetBonus.PartType.Head, 401, 403, 404);
		Add(Benefits.AdamantiteRanged, "ArmorSetBonus.AdamantiteRanged", ArmorSetBonus.PartType.Head, 402, 403, 404);
		Create(Benefits.Palladium, "ArmorSetBonus.Palladium").Set(1205, 1208, 1209).Set(1206, 1208, 1209).Set(1207, 1208, 1209)
			.Add();
		Create(Benefits.Orichalcum, "ArmorSetBonus.Orichalcum").Set(1210, 1213, 1214).Set(1211, 1213, 1214).Set(1212, 1213, 1214)
			.Add();
		Create(Benefits.Titanium, "ArmorSetBonus.Titanium").Set(1215, 1218, 1219).Set(1216, 1218, 1219).Set(1217, 1218, 1219)
			.Add();
		Create(Benefits.HallowedSummoner, "ArmorSetBonus.HallowedSummoner", ArmorSetBonus.PartType.Head).Set(new int[2] { 4873, 4899 }, new int[2] { 551, 4900 }, new int[2] { 552, 4901 }).Add();
		Create(Benefits.Hallowed, "ArmorSetBonus.Hallowed", ArmorSetBonus.PartType.Head).Set(new int[6] { 558, 553, 559, 4898, 4897, 4896 }, new int[2] { 551, 4900 }, new int[2] { 552, 4901 }).Add();
		Add(Benefits.CrystalAssassin, "ArmorSetBonus.CrystalNinja", 4982, 4983, 4984);
		Add(Benefits.Tiki, "ArmorSetBonus.Tiki", 1159, 1160, 1161);
		Add(Benefits.Spooky, "ArmorSetBonus.Spooky", 1832, 1833, 1834);
		Add(Benefits.Bee, "ArmorSetBonus.Bee", 2361, 2362, 2363);
		Add(Benefits.Spider, "ArmorSetBonus.Spider", 2370, 2371, 2372);
		Add(Benefits.Solar, "ArmorSetBonus.Solar", 2763, 2764, 2765);
		Add(Benefits.Vortex, "ArmorSetBonus.Vortex", 2757, 2758, 2759);
		Add(Benefits.Nebula, "ArmorSetBonus.Nebula", 2760, 2761, 2762);
		Add(Benefits.Stardust, "ArmorSetBonus.Stardust", 3381, 3382, 3383);
		Add(Benefits.Forbidden, "ArmorSetBonus.Forbidden", 3776, 3777, 3778);
		Add(Benefits.SquireTier2, "ArmorSetBonus.SquireTier2", 3800, 3801, 3802);
		Add(Benefits.ApprenticeTier2, "ArmorSetBonus.ApprenticeTier2", 3797, 3798, 3799);
		Add(Benefits.HuntressTier2, "ArmorSetBonus.HuntressTier2", 3803, 3804, 3805);
		Add(Benefits.MonkTier2, "ArmorSetBonus.MonkTier2", 3806, 3807, 3808);
		Add(Benefits.SquireTier3, "ArmorSetBonus.SquireTier3", 3871, 3872, 3873);
		Add(Benefits.ApprenticeTier3, "ArmorSetBonus.ApprenticeTier3", 3874, 3875, 3876);
		Add(Benefits.HuntressTier3, "ArmorSetBonus.HuntressTier3", 3877, 3878, 3879);
		Add(Benefits.MonkTier3, "ArmorSetBonus.MonkTier3", 3880, 3881, 3882);
		Add(Benefits.ObsidianOutlaw, "ArmorSetBonus.ObsidianOutlaw", 3266, 3267, 3268);
	}

	public static void BuildLookup()
	{
		ArmorSetBonus[] array = new ArmorSetBonus[0];
		SetsContaining = new ArmorSetBonus[ItemID.Count][];
		for (int i = 0; i < SetsContaining.Length; i++)
		{
			SetsContaining[i] = array;
		}
		foreach (IGrouping<int, ArmorSetBonus> item in from set in All
			group set by set.Head)
		{
			SetsContaining[item.Key] = item.ToArray();
		}
		foreach (IGrouping<int, ArmorSetBonus> item2 in from set in All
			group set by set.Body)
		{
			SetsContaining[item2.Key] = item2.ToArray();
		}
		foreach (IGrouping<int, ArmorSetBonus> item3 in from set in All
			group set by set.Legs)
		{
			SetsContaining[item3.Key] = item3.ToArray();
		}
		SetsContaining[0] = array;
	}

	public static ArmorSetBonus GetCompleteSet(ArmorSetBonus.QueryContext context)
	{
		ArmorSetBonus[] array = SetsContaining[context.HeadItem];
		foreach (ArmorSetBonus armorSetBonus in array)
		{
			if (armorSetBonus.QueryCount(context).Complete)
			{
				return armorSetBonus;
			}
		}
		array = SetsContaining[context.BodyItem];
		foreach (ArmorSetBonus armorSetBonus2 in array)
		{
			if (armorSetBonus2.QueryCount(context).Complete)
			{
				return armorSetBonus2;
			}
		}
		return null;
	}

	private static ArmorSetBonus.Builder Create(ArmorSetBonus.ArmorSetEffect effect, string textKey, ArmorSetBonus.PartType primaryPart = ArmorSetBonus.PartType.None)
	{
		return ArmorSetBonus.Create(effect, textKey, primaryPart);
	}

	public static void Add(ArmorSetBonus.ArmorSetEffect Effect, string TextKey, int Head, int Body, int Legs)
	{
		Create(Effect, TextKey).Set(Head, Body, Legs).Add();
	}

	public static void Add(ArmorSetBonus.ArmorSetEffect Effect, string TextKey, ArmorSetBonus.PartType PrimaryPart, int Head, int Body, int Legs)
	{
		Create(Effect, TextKey, PrimaryPart).Set(Head, Body, Legs).Add();
	}
}
