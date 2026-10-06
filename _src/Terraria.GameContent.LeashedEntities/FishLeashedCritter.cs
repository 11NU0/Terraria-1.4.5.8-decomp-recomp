using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities;

public class FishLeashedCritter : FlyerLeashedCritter
{
	public new static FishLeashedCritter Prototype = new FishLeashedCritter();

	public FishLeashedCritter()
	{
		anchorStyle = 3;
		minWaitTime = 120;
		maxFlySpeed = 0.5f;
		acceleration = 0.015f;
		hoverAmplitude = 4f;
		hoverPeriod = 0.003f;
		isAquatic = true;
	}

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		LeashedCritter._dummy.wet = true;
	}

	public override Vector2 GetDrawOffset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetBobbingOffset();
	}
}
