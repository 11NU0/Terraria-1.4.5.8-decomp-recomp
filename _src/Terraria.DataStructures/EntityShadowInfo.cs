using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public struct EntityShadowInfo
{
	public Vector2 Position;

	public float Rotation;

	public Vector2 Origin;

	public int Direction;

	public int GravityDirection;

	public int BodyFrameIndex;

	public Vector2 HeadgearOffset
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return Main.OffsetsPlayerHeadgear[BodyFrameIndex];
		}
	}

	public void CopyPlayer(Player player)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Position = player.position;
		Rotation = player.fullRotation;
		Origin = player.fullRotationOrigin;
		Direction = player.direction;
		GravityDirection = (int)player.gravDir;
		BodyFrameIndex = player.bodyFrame.Y / player.bodyFrame.Height;
	}
}
