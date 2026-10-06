using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Entrances;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon;

public class DungeonData
{
	public DungeonType Type;

	public int Iteration;

	public DungeonEntrance dungeonEntrance;

	public List<DungeonRoom> dungeonRooms = new List<DungeonRoom>();

	public List<DungeonHall> dungeonHalls = new List<DungeonHall>();

	public List<IDungeonFeature> dungeonFeatures = new List<IDungeonFeature>();

	public List<DungeonDoorData> dungeonDoorData = new List<DungeonDoorData>();

	public List<DungeonPlatformData> dungeonPlatformData = new List<DungeonPlatformData>();

	public List<DungeonBounds> protectedDungeonBounds = new List<DungeonBounds>();

	public bool makeNextPitTrapFlooded;

	public bool useSkewedDungeonEntranceHalls;

	public bool createdDungeonEntranceOnSurface;

	public double dungeonEntranceStrengthX;

	public double dungeonEntranceStrengthY;

	public double dungeonEntranceStrengthX2;

	public double dungeonEntranceStrengthY2;

	public Vector2D lastDungeonHall = Vector2D.Zero;

	public DungeonBounds dungeonBounds = new DungeonBounds();

	public DungeonBounds[] outerProgressionBounds = new DungeonBounds[0];

	public int[] wallVariants = new int[3];

	public int chandelierItemType;

	public int platformItemType;

	public int doorItemType;

	public int[] lanternStyles = new int[3];

	public int[] shelfStyles = new int[3];

	public int[] bannerStyles = new int[6];

	public double globalFeatureScalar = 1.0;

	public double dungeonStepScalar = 1.0;

	public double hallStrengthScalar = 1.0;

	public double hallStepScalar = 1.0;

	public double hallInteriorToExteriorRatio = 0.5;

	public double hallSlantVariantScalar = 1.0;

	public double roomStrengthScalar = 1.0;

	public double roomStepScalar = 1.0;

	public double roomInteriorToExteriorRatio = 0.5;

	public double roomSlantVariantScalar = 1.0;

	public DungeonGenVars genVars => GenVars.dungeonGenVars[Iteration];

	public double HallSizeScalar => (hallStrengthScalar + hallStepScalar) / 2.0;

	public double RoomSizeScalar => (roomStrengthScalar + roomStepScalar) / 2.0;

	public bool CanGenerateFeatureInArea(IDungeonFeature feature, int x, int y, int fluff)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		DungeonBounds dungeonBounds = new DungeonBounds();
		dungeonBounds.SetBounds(x - fluff, y - fluff, x + fluff, y + fluff);
		dungeonBounds.CalculateHitbox();
		return CanGenerateFeatureInArea(feature, dungeonBounds);
	}

	public bool CanGenerateFeatureInArea(IDungeonFeature feature, DungeonBounds bounds)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (!bounds.HasHitbox())
		{
			return false;
		}
		return CanGenerateFeatureInArea(feature, bounds.Hitbox);
	}

	public bool CanGenerateFeatureInArea(IDungeonFeature feature, Rectangle hitbox)
	{
		for (int i = hitbox.Left; i <= hitbox.Right; i++)
		{
			for (int j = hitbox.Top; j <= hitbox.Bottom; j++)
			{
				if (!CanGenerateFeatureAt(feature, i, j))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool CanGenerateFeatureAt(IDungeonFeature feature, int x, int y)
	{
		if (!WorldGen.InWorld(x, y, 5))
		{
			return false;
		}
		if (Main.tile[x, y].wall == 350)
		{
			return false;
		}
		if (dungeonEntrance.Bounds.Contains(x, y) && !dungeonEntrance.CanGenerateFeatureAt(this, feature, x, y))
		{
			return false;
		}
		for (int i = 0; i < protectedDungeonBounds.Count; i++)
		{
			if (protectedDungeonBounds[i].Contains(x, y))
			{
				return false;
			}
		}
		for (int j = 0; j < dungeonFeatures.Count; j++)
		{
			IDungeonFeature dungeonFeature = dungeonFeatures[j];
			if (dungeonFeature is DungeonFeature)
			{
				DungeonFeature dungeonFeature2 = (DungeonFeature)dungeonFeature;
				if (dungeonFeature2.generated && dungeonFeature2.Bounds.Contains(x, y) && !dungeonFeature2.CanGenerateFeatureAt(this, feature, x, y))
				{
					return false;
				}
			}
		}
		for (int k = 0; k < dungeonRooms.Count; k++)
		{
			DungeonRoom dungeonRoom = dungeonRooms[k];
			if (dungeonRoom.generated && dungeonRoom.OuterBounds.Contains(x, y) && !dungeonRoom.CanGenerateFeatureAt(this, feature, x, y))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsAnyRoomInSpot(out DungeonRoom roomFound, int x, int y, DungeonRoomSearchSettings settings)
	{
		roomFound = null;
		for (int i = 0; i < dungeonRooms.Count; i++)
		{
			DungeonRoom dungeonRoom = dungeonRooms[i];
			if (!DungeonUtils.RoomCanBeChosen(dungeonRoom, settings))
			{
				continue;
			}
			if (settings.UseOuterBounds)
			{
				if (!dungeonRoom.OuterBounds.ContainsWithFluff(x, y, settings.Fluff))
				{
					continue;
				}
			}
			else if (!dungeonRoom.InnerBounds.ContainsWithFluff(x, y, settings.Fluff))
			{
				continue;
			}
			roomFound = dungeonRoom;
			return true;
		}
		return false;
	}

	public bool IsAnyRoomInBounds(out DungeonRoom roomFound, DungeonBounds bounds, DungeonRoomSearchSettings settings)
	{
		roomFound = null;
		for (int i = 0; i < dungeonRooms.Count; i++)
		{
			DungeonRoom dungeonRoom = dungeonRooms[i];
			if (!DungeonUtils.RoomCanBeChosen(dungeonRoom, settings))
			{
				continue;
			}
			if (settings.UseOuterBounds)
			{
				if (!dungeonRoom.OuterBounds.Intersects(bounds))
				{
					continue;
				}
			}
			else if (!dungeonRoom.InnerBounds.Intersects(bounds))
			{
				continue;
			}
			roomFound = dungeonRoom;
			return true;
		}
		return false;
	}

	public DungeonData()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
	}
}
