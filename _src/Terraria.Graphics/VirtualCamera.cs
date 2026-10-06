using Microsoft.Xna.Framework;

namespace Terraria.Graphics;

public struct VirtualCamera(Player player)
{
	public readonly Player Player = player;

	public Vector2 Position
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return Center - Size * 0.5f;
		}
	}

	public Vector2 Size
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)Main.maxScreenW, (float)Main.maxScreenH);
		}
	}

	public Vector2 Center
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Player.Center;
		}
	}
}
