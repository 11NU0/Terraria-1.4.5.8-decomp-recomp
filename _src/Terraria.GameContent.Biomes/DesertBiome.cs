using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Biomes.Desert;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class DesertBiome : MicroBiome
{
	[JsonProperty("ChanceOfEntrance")]
	public double ChanceOfEntrance = 0.3333;

	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		DesertDescription desertDescription = DesertDescription.CreateFromPlacement(origin);
		if (!desertDescription.IsValid)
		{
			return false;
		}
		ExportDescriptionToEngine(desertDescription);
		SandMound.Place(desertDescription, progress, 0f, 0.1f);
		desertDescription.UpdateSurfaceMap();
		if (!Main.tenthAnniversaryWorld && GenBase._random.NextDouble() <= ChanceOfEntrance && !WorldGen.SecretSeed.extraLiquid.Enabled)
		{
			switch (GenBase._random.Next(4))
			{
			case 0:
				ChambersEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
				break;
			case 1:
				AnthillEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
				break;
			case 2:
				LarvaHoleEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
				break;
			case 3:
				PitEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
				break;
			}
		}
		DesertHive.Place(desertDescription, progress, 0.2f, 0.75f);
		CleanupArea(desertDescription.Hive, progress, 0.75f, 1f);
		int x = desertDescription.CombinedArea.X;
		int width = desertDescription.CombinedArea.Width;
		Rectangle combinedArea = desertDescription.CombinedArea;
		Rectangle area = new Rectangle(x, 50, width, combinedArea.Bottom - 20);
		structures.AddStructure(area, 10);
		return true;
	}

	private static void ExportDescriptionToEngine(DesertDescription description)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		GenVars.UndergroundDesertLocation = description.CombinedArea;
		GenVars.UndergroundDesertLocation.Inflate(10, 10);
		GenVars.UndergroundDesertHiveLocation = description.Hive;
	}

	private static void CleanupArea(Rectangle area, GenerationProgress progress, float progressMin, float progressMax)
	{
		int num = 20 - area.Left;
		int num2 = num + area.Right + 20;
		for (int i = -20 + area.Left; i < area.Right + 20; i++)
		{
			progress.Set((float)(i + num) / (float)num2, progressMin, progressMax);
			for (int j = -20 + area.Top; j < area.Bottom + 20; j++)
			{
				if (i > 0 && i < Main.maxTilesX - 1 && j > 0 && j < Main.maxTilesY - 1)
				{
					WorldGen.SquareWallFrame(i, j);
					WorldUtils.TileFrame(i, j, frameNeighbors: true);
				}
			}
		}
	}
}
