using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding;

public class SimpleStructure : GenStructure
{
	private int[,] _data;

	private int _width;

	private int _height;

	private GenAction[] _actions;

	private bool _xMirror;

	private bool _yMirror;

	public int Width => _width;

	public int Height => _height;

	public SimpleStructure(params string[] data)
	{
		ReadData(data);
	}

	public SimpleStructure(string data)
	{
		ReadData(data.Split('\n'));
	}

	private void ReadData(string[] lines)
	{
		_height = lines.Length;
		_width = lines[0].Length;
		_data = new int[_width, _height];
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				int num = lines[i][j];
				if (num >= 48 && num <= 57)
				{
					_data[j, i] = num - 48;
				}
				else
				{
					_data[j, i] = -1;
				}
			}
		}
	}

	public SimpleStructure SetActions(params GenAction[] actions)
	{
		_actions = actions;
		return this;
	}

	public SimpleStructure Mirror(bool horizontalMirror, bool verticalMirror)
	{
		_xMirror = horizontalMirror;
		_yMirror = verticalMirror;
		return this;
	}

	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (!structures.CanPlace(new Rectangle(origin.X, origin.Y, _width, _height)))
		{
			return false;
		}
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				int num = (_xMirror ? (-i) : i);
				int num2 = (_yMirror ? (-j) : j);
				if (_data[i, j] != -1 && !_actions[_data[i, j]].Apply(origin, num + origin.X, num2 + origin.Y))
				{
					return false;
				}
			}
		}
		structures.AddProtectedStructure(new Rectangle(origin.X, origin.Y, _width, _height));
		return true;
	}
}
