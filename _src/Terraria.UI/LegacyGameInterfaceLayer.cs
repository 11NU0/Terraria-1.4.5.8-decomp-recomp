using Terraria.Testing;

namespace Terraria.UI;

public class LegacyGameInterfaceLayer : GameInterfaceLayer
{
	private GameInterfaceDrawMethod _drawMethod;

	public LegacyGameInterfaceLayer(string name, GameInterfaceDrawMethod drawMethod, InterfaceScaleType scaleType = InterfaceScaleType.Game)
		: base(name, scaleType)
	{
		Invariant.Assert(drawMethod != null, "drawMethod should never be null.");
		_drawMethod = drawMethod;
	}

	protected override bool DrawSelf()
	{
		return _drawMethod();
	}
}
