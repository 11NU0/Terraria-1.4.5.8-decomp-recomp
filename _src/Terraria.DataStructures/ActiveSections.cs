using System;
using Microsoft.Xna.Framework;
using Terraria.Testing;

namespace Terraria.DataStructures;

public static class ActiveSections
{
	public static readonly uint SectionInactiveTime = 60u;

	private static uint[,] LastActiveTime = new uint[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];

	public static event Action<Point> SectionActivated;

	public static void CheckSection(Vector2 position, int fluff = 1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		int sectionX = Netplay.GetSectionX((int)(position.X / 16f));
		int sectionY = Netplay.GetSectionY((int)(position.Y / 16f));
		for (int i = sectionX - fluff; i < sectionX + fluff + 1; i++)
		{
			for (int j = sectionY - fluff; j < sectionY + fluff + 1; j++)
			{
				if (i >= 0 && i < Main.maxSectionsX && j >= 0 && j < Main.maxSectionsY)
				{
					bool flag = IsSectionActive(new Point(i, j));
					LastActiveTime[i, j] = Main.GameUpdateCount;
					if (!flag)
					{
						SectionActivated(new Point(i, j));
					}
				}
			}
		}
	}

	public static bool IsSectionActive(Point sectionCoords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		sectionCoords = sectionCoords.ClampSectionCoords();
		return LastActiveTime[sectionCoords.X, sectionCoords.Y] + SectionInactiveTime >= Main.GameUpdateCount;
	}

	public static int TimeTillInactive(Point sectionCoords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		sectionCoords = sectionCoords.ClampSectionCoords();
		return (int)Math.Max(0L, (long)(LastActiveTime[sectionCoords.X, sectionCoords.Y] + SectionInactiveTime) - (long)Main.GameUpdateCount);
	}

	public static void Reset()
	{
		Array.Clear(LastActiveTime, 0, LastActiveTime.Length);
	}

	public static Point ClampSectionCoords(this Point point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Point(Utils.Clamp(point.X, 0, Main.maxSectionsX), Utils.Clamp(point.Y, 0, Main.maxSectionsY));
	}

	internal static void AddGameplaySnapshotComponents()
	{
		StateSnapshot.Gameplay.AddCollection("ActiveSections.LastActiveTime", LastActiveTime);
	}
}
