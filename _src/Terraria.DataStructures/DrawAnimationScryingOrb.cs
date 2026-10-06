using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures;

public class DrawAnimationScryingOrb : DrawAnimation
{
	private enum State
	{
		Moving,
		Frozen,
		FrozenCenter
	}

	private State _state;

	private int _nextStateCounter;

	private int _dir = 1;

	public override void Update()
	{
		if (++FrameCounter < TicksPerFrame)
		{
			return;
		}
		FrameCounter = 0;
		if (--_nextStateCounter <= 0)
		{
			if (_state != State.Moving)
			{
				_state = State.Moving;
				_nextStateCounter = Main.rand.Next(3, 9);
				if (Main.rand.Next(4) == 0)
				{
					_dir *= -1;
				}
			}
			else
			{
				_state = ((Main.rand.Next(4) != 0) ? State.Frozen : State.FrozenCenter);
				_nextStateCounter = Main.rand.Next(7, 10);
			}
		}
		else if (_state == State.Moving)
		{
			Frame += _dir;
			if (Frame >= FrameCount)
			{
				Frame = 1;
			}
			else if (Frame <= 0)
			{
				Frame = FrameCount - 1;
			}
		}
	}

	public override Rectangle GetFrame(Texture2D texture, int frameCounterOverride = -1)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		int frameY = ((_state != State.FrozenCenter) ? Frame : 0);
		if (frameCounterOverride >= 0)
		{
			frameY = frameCounterOverride;
		}
		return texture.Frame(1, FrameCount, 0, frameY, 0, -2);
	}
}
