using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Map;

public class MapIconOverlay
{
	private readonly List<IMapLayer> _layers = new List<IMapLayer>();

	public MapIconOverlay AddLayer(IMapLayer layer)
	{
		_layers.Add(layer);
		return this;
	}

	public void Draw(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale, int alpha, ref string text)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		MapOverlayDrawContext context = new MapOverlayDrawContext(mapPosition, mapOffset, clippingRect, mapScale, drawScale, (float)alpha / 255f);
		foreach (IMapLayer layer in _layers)
		{
			layer.Draw(ref context, ref text);
		}
	}
}
