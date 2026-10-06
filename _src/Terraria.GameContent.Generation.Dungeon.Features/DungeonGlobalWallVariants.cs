using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalWallVariants : GlobalDungeonFeature
{
	public DungeonGlobalWallVariants(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		WallVariants(data);
		generated = true;
		return true;
	}

	public void WallVariants(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int[] wallVariants = data.wallVariants;
		int num = wallVariants.Length;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < num; j++)
			{
				int num2 = genRand.Next(40, 240);
				int num3 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num4 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
				for (int k = num3 - num2; k < num3 + num2; k++)
				{
					for (int l = num4 - num2; l < num4 + num2; l++)
					{
						if (!((double)l <= Main.worldSurface) && WorldGen.InWorld(k, l, 2))
						{
							int num5 = Math.Abs(num3 - k);
							int num6 = Math.Abs(num4 - l);
							if (!(Math.Sqrt(num5 * num5 + num6 * num6) >= (double)((float)num2 * 0.4f)) && Main.wallDungeon[Main.tile[k, l].wall])
							{
								SpreadWallDungeon(data, k, l, (ushort)wallVariants[j]);
							}
						}
					}
				}
			}
		}
	}

	public void SpreadWallDungeon(DungeonData data, int x, int y, ushort wallType, bool dungeonWallOnly = true)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(x, y))
		{
			return;
		}
		ushort num = wallType;
		List<Point> list = new List<Point>();
		List<Point> list2 = new List<Point>();
		HashSet<Point> hashSet = new HashSet<Point>();
		list2.Add(new Point(x, y));
		while (list2.Count > 0)
		{
			list.Clear();
			list.AddRange(list2);
			list2.Clear();
			while (list.Count > 0)
			{
				Point val = list[0];
				if (!WorldGen.InWorld(val.X, val.Y, 1))
				{
					list.Remove(val);
					continue;
				}
				hashSet.Add(val);
				list.Remove(val);
				Tile tile = Main.tile[val.X, val.Y];
				if (tile.wall == 0 || tile.wall == num || tile.wall == 244 || tile.wall == 62 || !data.CanGenerateFeatureAt(this, val.X, val.Y))
				{
					continue;
				}
				if (data.dungeonEntrance.Bounds.Contains(val.X, val.Y))
				{
					if (tile.wall != data.dungeonEntrance.settings.StyleData.BrickWallType)
					{
						continue;
					}
				}
				else if (dungeonWallOnly && tile.wall != data.genVars.brickWallType)
				{
					continue;
				}
				if (!WorldGen.SolidTile(val.X, val.Y))
				{
					tile.wall = num;
					Point item = new Point(val.X - 1, val.Y);
					if (!hashSet.Contains(item))
					{
						list2.Add(item);
					}
					item = new Point(val.X + 1, val.Y);
					if (!hashSet.Contains(item))
					{
						list2.Add(item);
					}
					item = new Point(val.X, val.Y - 1);
					if (!hashSet.Contains(item))
					{
						list2.Add(item);
					}
					item = new Point(val.X, val.Y + 1);
					if (!hashSet.Contains(item))
					{
						list2.Add(item);
					}
				}
				else if (tile.active())
				{
					tile.wall = num;
				}
			}
		}
	}
}
