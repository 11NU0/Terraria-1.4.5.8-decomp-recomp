namespace Terraria.GameContent;

public static class Luck
{
	public static int RollLuck(float luck, int range)
	{
		if (luck > 0f && Main.rand.NextFloat() < luck)
		{
			return Main.rand.Next(Main.rand.Next(range / 2, range));
		}
		if (luck < 0f && Main.rand.NextFloat() < 0f - luck)
		{
			return Main.rand.Next(Main.rand.Next(range, range * 2));
		}
		return Main.rand.Next(range);
	}

	public static int RollBadLuck(float luck, int range)
	{
		if (luck > 0f && Main.rand.NextFloat() < luck)
		{
			return Main.rand.Next(Main.rand.Next(range, range * 2));
		}
		if (luck < 0f && Main.rand.NextFloat() < 0f - luck)
		{
			return Main.rand.Next(Main.rand.Next(range / 2, range));
		}
		return Main.rand.Next(range);
	}

	public static int RollOnlyBadLuck(float luck, int range)
	{
		if (luck < 0f && Main.rand.NextFloat() < 0f - luck)
		{
			return Main.rand.Next(Main.rand.Next(range / 2, range));
		}
		return Main.rand.Next(range);
	}

	public static int RollBadLuckExtreme(float luck, int range)
	{
		if (luck > 0f && Main.rand.NextFloat() < luck)
		{
			return Main.rand.Next(range * 10);
		}
		if (luck < 0f && Main.rand.NextFloat() < 0f - luck)
		{
			return Main.rand.Next(range / 10);
		}
		return Main.rand.Next(range);
	}

	public static int RollOnlyBadLuckExtreme(float luck, int range)
	{
		if (luck < 0f && Main.rand.NextFloat() < 0f - luck)
		{
			return Main.rand.Next(range / 10);
		}
		return -1;
	}
}
