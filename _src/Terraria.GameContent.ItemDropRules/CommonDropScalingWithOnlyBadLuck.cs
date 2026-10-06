namespace Terraria.GameContent.ItemDropRules;

public class CommonDropScalingWithOnlyBadLuck : CommonDrop
{
	public CommonDropScalingWithOnlyBadLuck(int itemId, int chanceDenominator, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, int chanceNumerator = 1)
		: base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, chanceNumerator)
	{
	}

	public CommonDropScalingWithOnlyBadLuck(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum)
		: base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum)
	{
	}

	public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		if (info.player.RollOnlyBadLuck(chanceDenominator) < chanceNumerator)
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
