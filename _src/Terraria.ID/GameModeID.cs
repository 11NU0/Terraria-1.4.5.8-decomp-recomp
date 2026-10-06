namespace Terraria.ID;

internal class GameModeID
{
	public const short Normal = 0;

	public const short Expert = 1;

	public const short Master = 2;

	public const short Creative = 3;

	public const short Count = 4;

	public static bool IsValid(int gameMode)
	{
		if (gameMode >= 0)
		{
			return gameMode < 4;
		}
		return false;
	}
}
