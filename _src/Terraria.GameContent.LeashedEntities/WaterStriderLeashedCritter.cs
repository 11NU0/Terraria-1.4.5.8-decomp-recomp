using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities;

internal class WaterStriderLeashedCritter : JumperLeashedCritter
{
	public new static WaterStriderLeashedCritter Prototype = new WaterStriderLeashedCritter();

	public WaterStriderLeashedCritter()
	{
		minWaitTime = 240;
		maxWaitTime = 540;
		strayingRangeInBlocksX = 5;
		strayingRangeInBlocksY = 12;
		maxJumpWidth = 32f;
		minJumpWidth = 8f;
		maxJumpHeight = 0f;
		maxJumpDuration = 7f;
		jumpCooldown = 120;
		canStandOnWater = true;
		drawBubble = false;
	}

	public override Vector2 GetDrawOffset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Vector2 drawOffset = base.GetDrawOffset();
		Point val = Center.ToTileCoordinates();
		if (WorldGen.AnyLiquidAt(val, 0))
		{
			return drawOffset;
		}
		for (int i = 0; i < 2; i++)
		{
			val.Y++;
			byte liquid = Framing.GetTileSafely(val).liquid;
			if (liquid != 0)
			{
				drawOffset.Y = (255 - liquid) / 16;
				break;
			}
		}
		return drawOffset;
	}
}
