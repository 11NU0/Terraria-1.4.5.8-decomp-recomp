using Microsoft.Xna.Framework;
using Terraria.Enums;

namespace Terraria.DataStructures;

public struct NPCAimedTarget
{
	public NPCTargetType Type;

	public Rectangle Hitbox;

	public int Width;

	public int Height;

	public Vector2 Position;

	public Vector2 Velocity;

	public bool Invalid => Type == NPCTargetType.None;

	public Vector2 Center
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return Position + Size / 2f;
		}
	}

	public Vector2 Size
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)Width, (float)Height);
		}
	}

	public NPCAimedTarget(NPC npc)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Type = NPCTargetType.NPC;
		Hitbox = npc.Hitbox;
		Width = npc.width;
		Height = npc.height;
		Position = npc.position;
		Velocity = npc.velocity;
	}

	public NPCAimedTarget(Player player, bool ignoreTank = true)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		Type = NPCTargetType.Player;
		Hitbox = player.Hitbox;
		Width = player.width;
		Height = player.height;
		Position = player.position;
		Velocity = player.velocity;
		if (!ignoreTank && player.tankPet > -1)
		{
			Projectile projectile = Main.projectile[player.tankPet];
			Type = NPCTargetType.PlayerTankPet;
			Hitbox = projectile.Hitbox;
			Width = projectile.width;
			Height = projectile.height;
			Position = projectile.position;
			Velocity = projectile.velocity;
		}
	}
}
