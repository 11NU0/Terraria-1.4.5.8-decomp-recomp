namespace Terraria.ID;

public static class ImmunityCooldownID
{
	public static class Sets
	{
		public struct BoolSet
		{
			private readonly bool[] _arr;

			public bool this[int idx]
			{
				get
				{
					return _arr[idx + 1];
				}
				set
				{
					_arr[idx + 1] = value;
				}
			}

			public BoolSet(int count)
			{
				_arr = new bool[count + 1];
			}
		}

		public static BoolSet Retaliate = CreateBoolSet(General, BossNoCheese, PaladinsShield);

		public static BoolSet Counter = CreateBoolSet(General, BossNoCheese);

		public static BoolSet TeamDamageShare = CreateBoolSet(General, BossNoCheese);

		public static BoolSet ImmuneTimerOnlyLimitsEffects = CreateBoolSet(PaladinsShield);

		public static BoolSet CreateBoolSet(params int[] types)
		{
			BoolSet result = new BoolSet(Count);
			foreach (int idx in types)
			{
				result[idx] = true;
			}
			return result;
		}
	}

	public static readonly int General = -1;

	public static readonly int TileContactDamage = 0;

	public static readonly int BossNoCheese = 1;

	public static readonly int LegacyUnused2 = 2;

	public static readonly int WrongBugNet = 3;

	public static readonly int Lava = 4;

	public static readonly int PaladinsShield = 5;

	public static readonly int Count = 6;
}
