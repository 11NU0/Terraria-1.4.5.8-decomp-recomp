namespace Terraria.DataStructures;

public class BackgroundVariant
{
	private readonly int[] _backgrounds = new int[3] { -1, -1, -1 };

	public int[] Backgrounds => _backgrounds;

	public bool HasAny
	{
		get
		{
			if (_backgrounds[0] == -1 && _backgrounds[1] == -1)
			{
				return _backgrounds[2] != -1;
			}
			return true;
		}
	}

	public void Set(int far, int middle, int near)
	{
		_backgrounds[0] = far;
		_backgrounds[1] = middle;
		_backgrounds[2] = near;
	}

	public void Clear()
	{
		Set(-1, -1, -1);
	}
}
