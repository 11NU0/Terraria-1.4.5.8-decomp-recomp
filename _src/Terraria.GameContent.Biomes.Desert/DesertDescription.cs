using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert;

public class DesertDescription
{
	public static readonly DesertDescription Invalid = new DesertDescription
	{
		IsValid = false
	};

	private static readonly Vector2D DefaultBlockScale = new Vector2D(4.0, 2.0);

	private const int SCAN_PADDING = 5;

	public Rectangle CombinedArea
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		private set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public Rectangle Desert
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		private set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public Rectangle Hive
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		private set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public Vector2D BlockScale
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		private set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public int BlockColumnCount { get; private set; }

	public int BlockRowCount { get; private set; }

	public bool IsValid { get; private set; }

	public SurfaceMap Surface { get; private set; }

	private DesertDescription()
	{
	}

	public void UpdateSurfaceMap()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Rectangle combinedArea = CombinedArea;
		Surface = SurfaceMap.FromArea(combinedArea.Left - 5, CombinedArea.Width + 10);
	}

	public static DesertDescription CreateFromPlacement(Point origin)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		Vector2D defaultBlockScale = DefaultBlockScale;
		double num = (double)Main.maxTilesX / 4200.0;
		int num2 = (int)(80.0 * num);
		int num3 = (int)((WorldGen.genRand.NextDouble() * 0.5 + 1.5) * 170.0 * num);
		if (WorldGen.remixWorldGen)
		{
			num3 = (int)(340.0 * num);
		}
		int num4 = (int)(defaultBlockScale.X * (double)num2);
		int num5 = (int)(defaultBlockScale.Y * (double)num3);
		origin.X -= num4 / 2;
		SurfaceMap surfaceMap = SurfaceMap.FromArea(origin.X - 5, num4 + 10);
		if (RowHasInvalidTiles(origin.X, surfaceMap.Bottom, num4))
		{
			return Invalid;
		}
		int num6 = (int)(surfaceMap.Average + (double)surfaceMap.Bottom) / 2;
		origin.Y = num6 + WorldGen.genRand.Next(40, 60);
		int num7 = 0;
		if (Main.tenthAnniversaryWorld)
		{
			num7 = (int)(20.0 * num);
		}
		return new DesertDescription
		{
			CombinedArea = new Rectangle(origin.X, num6, num4, origin.Y + num5 - num6),
			Hive = new Rectangle(origin.X, origin.Y + num7, num4, num5 - num7),
			Desert = new Rectangle(origin.X, num6, num4, origin.Y + num5 / 2 - num6 + num7),
			BlockScale = defaultBlockScale,
			BlockColumnCount = num2,
			BlockRowCount = num3,
			Surface = surfaceMap,
			IsValid = true
		};
	}

	private static bool RowHasInvalidTiles(int startX, int startY, int width)
	{
		if (GenVars.skipDesertTileCheck)
		{
			return false;
		}
		for (int i = startX; i < startX + width; i++)
		{
			ushort type = Main.tile[i, startY].type;
			if ((!WorldGen.notTheBees || WorldGen.remixWorldGen) && (type == 59 || type == 60))
			{
				return true;
			}
			if (type == 161 || type == 147)
			{
				return true;
			}
		}
		return false;
	}

	static DesertDescription()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
	}
}
