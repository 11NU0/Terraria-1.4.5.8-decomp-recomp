using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.ID;

namespace Terraria.WorldBuilding;

public class StructureMap
{
	[JsonProperty]
	private readonly List<Rectangle> _structures = new List<Rectangle>(2048);

	[JsonProperty]
	private readonly List<Rectangle> _protectedStructures = new List<Rectangle>(2048);

	private readonly object _lock = new object();

	public bool CanPlace(Rectangle area, int padding = 0)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return CanPlace(area, TileID.Sets.GeneralPlacementTiles, padding);
	}

	public bool CanPlace(Rectangle area, bool[] validTiles, int padding = 0)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			if (area.X < 0 || area.Y < 0 || area.X + area.Width > Main.maxTilesX - 1 || area.Y + area.Height > Main.maxTilesY - 1)
			{
				return false;
			}
			Rectangle val = new Rectangle(area.X - padding, area.Y - padding, area.Width + padding * 2, area.Height + padding * 2);
			for (int i = 0; i < _protectedStructures.Count; i++)
			{
				if (val.Intersects(_protectedStructures[i]))
				{
					return false;
				}
			}
			for (int j = val.X; j < val.X + val.Width; j++)
			{
				for (int k = val.Y; k < val.Y + val.Height; k++)
				{
					if (Main.tile[j, k].active())
					{
						ushort type = Main.tile[j, k].type;
						if (!validTiles[type])
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}

	public Rectangle GetBoundingBox()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			if (_structures.Count == 0)
			{
				return Rectangle.Empty;
			}
			Point val = new Point(_structures.Min((Rectangle rect) => rect.Left), _structures.Min((Rectangle rect) => rect.Top));
			Point val2 = new Point(_structures.Max((Rectangle rect) => rect.Right), _structures.Max((Rectangle rect) => rect.Bottom));
			return new Rectangle(val.X, val.Y, val2.X - val.X, val2.Y - val.Y);
		}
	}

	public void AddStructure(Rectangle area, int padding = 0)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			area.Inflate(padding, padding);
			_structures.Add(area);
		}
	}

	public void AddProtectedStructure(Rectangle area, int padding = 0)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			area.Inflate(padding, padding);
			_structures.Add(area);
			_protectedStructures.Add(area);
		}
	}

	public void Reset()
	{
		lock (_lock)
		{
			_protectedStructures.Clear();
		}
	}
}
