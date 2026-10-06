using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalLights : GlobalDungeonFeature
{
	public DungeonGlobalLights(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Lights(data);
		generated = true;
		return true;
	}

	public void Lights(DungeonData data)
	{
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		float num = (float)Main.maxTilesX / 4200f;
		int num2 = 0;
		int num3 = 1000;
		int num4 = 0;
		int num5 = (int)((double)(28f * num) * data.globalFeatureScalar);
		while (num4 < num5)
		{
			num2++;
			int num6 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int num7 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
			if (DungeonUtils.IsConsideredDungeonWall(Main.tile[num6, num7].wall))
			{
				for (int num8 = num7; num8 > data.dungeonBounds.Top; num8--)
				{
					if (Main.tile[num6, num8 - 1].active() && DungeonUtils.IsConsideredDungeonTile(Main.tile[num6, num8 - 1].type) && data.CanGenerateFeatureAt(this, num6, num8) && (data.dungeonEntrance.Bounds.Contains(num6, num8) || DungeonUtils.IsConsideredDungeonWall(Main.tile[num6, num8].wall)))
					{
						bool flag = false;
						for (int i = num6 - 15; i < num6 + 15; i++)
						{
							for (int j = num8 - 15; j < num8 + 15; j++)
							{
								if (i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY && (Main.tile[i, j].type == 42 || Main.tile[i, j].type == 34))
								{
									flag = true;
									break;
								}
							}
						}
						if (Main.tile[num6 - 1, num8].active() || Main.tile[num6 + 1, num8].active() || Main.tile[num6 - 1, num8 + 1].active() || Main.tile[num6 + 1, num8 + 1].active() || Main.tile[num6, num8 + 2].active())
						{
							flag = true;
						}
						if (flag)
						{
							break;
						}
						bool flag2 = false;
						if (!flag2 && genRand.Next(7) == 0)
						{
							bool flag3 = false;
							for (int k = 0; k < 15; k++)
							{
								if (WorldGen.SolidTile(num6, num8 + k))
								{
									flag3 = true;
									break;
								}
							}
							if (!flag3)
							{
								DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, Main.tile[num6, num8].wall);
								if (styleForWall != null && styleForWall.ChandelierItemTypes != null)
								{
									int num9 = 0;
									if (styleForWall.ChandelierItemTypes.Length == 0 || styleForWall.Style == 0)
									{
										num9 = data.chandelierItemType;
										if (Main.tile[num6, num8].wall >= 94 && Main.tile[num6, num8].wall <= 105)
										{
											num9 = 5744;
										}
									}
									else
									{
										num9 = styleForWall.ChandelierItemTypes[genRand.Next(styleForWall.ChandelierItemTypes.Length)];
									}
									PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[num9];
									if (placementDetails.tileType >= 0)
									{
										WorldGen.PlaceChand(num6, num8, (ushort)placementDetails.tileType, placementDetails.tileStyle);
										if (Main.tile[num6, num8].type == 34)
										{
											flag2 = true;
											num2 = 0;
											num4++;
											Lights_GenerateSwitch(num6, num8);
										}
									}
								}
							}
						}
						if (flag2)
						{
							break;
						}
						DungeonGenerationStyleData styleForWall2 = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, Main.tile[num6, num8].wall);
						ushort num10 = 42;
						int num11 = 0;
						if (styleForWall2 == null || styleForWall2.LanternItemTypes != null)
						{
							if (styleForWall2 == null || styleForWall2.Style == 0 || styleForWall2.LanternItemTypes.Length == 0)
							{
								num11 = data.lanternStyles[0];
								if (Main.tile[num6, num8].wall == data.wallVariants[1])
								{
									num11 = data.lanternStyles[1];
								}
								if (Main.tile[num6, num8].wall == data.wallVariants[2])
								{
									num11 = data.lanternStyles[2];
								}
								if (genRand.Next(3) == 0 && Main.tile[num6, num8].wall >= 94 && Main.tile[num6, num8].wall <= 105)
								{
									num10 = 42;
									num11 = 53;
								}
							}
							else
							{
								PlacementDetails placementDetails2 = ItemID.Sets.DerivedPlacementDetails[styleForWall2.LanternItemTypes[genRand.Next(styleForWall2.LanternItemTypes.Length)]];
								num10 = (ushort)placementDetails2.tileType;
								num11 = placementDetails2.tileStyle;
							}
							WorldGen.Place1x2Top(num6, num8, num10, num11);
							if (Main.tile[num6, num8].type == num10)
							{
								flag2 = true;
								num2 = 0;
								num4++;
								Lights_GenerateSwitch(num6, num8);
							}
							break;
						}
					}
				}
			}
			if (num2 > num3)
			{
				num4++;
				num2 = 0;
			}
		}
	}

	private Point Lights_GenerateSwitch(int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		Point result = Point.Zero;
		for (int i = 0; i < 1000; i++)
		{
			int num = x + WorldGen.genRand.Next(-12, 13);
			int num2 = y + WorldGen.genRand.Next(3, 21);
			if (Main.tile[num, num2].active() || Main.tile[num, num2 + 1].active() || !DungeonUtils.IsConsideredDungeonTile(Main.tile[num - 1, num2].type) || !DungeonUtils.IsConsideredDungeonTile(Main.tile[num + 1, num2].type) || !Collision.CanHit(new Point(num * 16, num2 * 16), 16, 16, new Point(x * 16, y * 16 + 1), 16, 16))
			{
				continue;
			}
			if (((WorldGen.SolidTile(num - 1, num2) && Main.tile[num - 1, num2].type != 10) || (WorldGen.SolidTile(num + 1, num2) && Main.tile[num + 1, num2].type != 10) || WorldGen.SolidTile(num, num2 + 1)) && DungeonUtils.IsConsideredDungeonWall(Main.tile[num, num2].wall) && (DungeonUtils.IsConsideredDungeonTile(Main.tile[num - 1, num2].type) || DungeonUtils.IsConsideredDungeonTile(Main.tile[num + 1, num2].type)))
			{
				WorldGen.PlaceTile(num, num2, 136, mute: true);
				result = new Point(num, num2);
			}
			if (!Main.tile[num, num2].active())
			{
				continue;
			}
			while (num != x || num2 != y)
			{
				Main.tile[num, num2].wire(wire: true);
				if (num > x)
				{
					num--;
				}
				if (num < x)
				{
					num++;
				}
				Main.tile[num, num2].wire(wire: true);
				if (num2 > y)
				{
					num2--;
				}
				if (num2 < y)
				{
					num2++;
				}
				Main.tile[num, num2].wire(wire: true);
			}
			if (WorldGen.genRand.Next(3) > 0)
			{
				Main.tile[x, y].frameX = 18;
				Main.tile[x, y + 1].frameX = 18;
			}
			break;
		}
		return result;
	}
}
