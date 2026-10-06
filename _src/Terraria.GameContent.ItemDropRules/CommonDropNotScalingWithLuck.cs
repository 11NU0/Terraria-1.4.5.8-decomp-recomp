namespace Terraria.GameContent.ItemDropRules;

public class CommonDropNotScalingWithLuck : CommonDrop
{
	public CommonDropNotScalingWithLuck(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum)
		: base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum)
	{
	}

	public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		if (info.rng.Next(chanceDenominator) < chanceNumerator)
		{
			CommonCode.DropItemFromNPC(info.npc, itemId, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.Success
			};
		}
		return new ItemDropAttemptResult
		{
			State = ItemDropAttemptResultState.FailedRandomRoll
		};
	}
}
