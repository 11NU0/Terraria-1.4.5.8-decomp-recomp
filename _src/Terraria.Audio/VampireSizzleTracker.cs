namespace Terraria.Audio;

public class VampireSizzleTracker
{
	private int _playerIndex;

	public VampireSizzleTracker(int whoAmI)
	{
		_playerIndex = whoAmI;
	}

	public bool IsActiveAndInGame()
	{
		if (Main.gameMenu || !Main.vampireSeed)
		{
			return false;
		}
		return Main.player[_playerIndex].sunScorchCounter > 0;
	}
}
