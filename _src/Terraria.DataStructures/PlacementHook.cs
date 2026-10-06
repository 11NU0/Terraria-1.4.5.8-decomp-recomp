namespace Terraria.DataStructures;

public struct PlacementHook(PlacementHook.HookFormat hook, int badReturn, int badResponse, bool processedCoordinates)
{
	public delegate int HookFormat(int x, int y, int type, int style, int direction, int alternate);

	public HookFormat hook = hook;

	public int badReturn = badReturn;

	public int badResponse = badResponse;

	public bool processedCoordinates = processedCoordinates;

	public static PlacementHook Empty = new PlacementHook(null, 0, 0, processedCoordinates: false);

	public const int Response_AllInvalid = 0;

	public static bool operator ==(PlacementHook first, PlacementHook second)
	{
		if (first.hook == second.hook && first.badResponse == second.badResponse && first.badReturn == second.badReturn)
		{
			return first.processedCoordinates == second.processedCoordinates;
		}
		return false;
	}

	public static bool operator !=(PlacementHook first, PlacementHook second)
	{
		if (!(first.hook != second.hook) && first.badResponse == second.badResponse && first.badReturn == second.badReturn)
		{
			return first.processedCoordinates != second.processedCoordinates;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj is PlacementHook)
		{
			return this == (PlacementHook)obj;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
