using ReLogic.Reflection;

namespace Terraria.ID;

public class SurfaceBackgroundID
{
	public static class Sets
	{
		public static SetFactory Factory = new SetFactory(16);

		public static bool[] IsDesertVariant = Factory.CreateBoolSet(false, 2, 5, 13, 14);

		public static bool[] IsForest = Factory.CreateBoolSet(false, 0, 10, 11, 12);
	}

	public const int Forest1 = 0;

	public const int Corruption = 1;

	public const int Desert = 2;

	public const int Jungle = 3;

	public const int Ocean = 4;

	public const int CorruptDesert = 5;

	public const int Hallow = 6;

	public const int Snow = 7;

	public const int Crimson = 8;

	public const int Mushroom = 9;

	public const int Forest2 = 10;

	public const int Forest3 = 11;

	public const int Forest4 = 12;

	public const int HallowDesert = 13;

	public const int CrimsonDesert = 14;

	public const int Empty = 15;

	public const int Count = 16;

	public static readonly IdDictionary Search = IdDictionary.Create<SurfaceBackgroundID, int>();
}
