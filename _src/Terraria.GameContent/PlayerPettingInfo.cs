using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public struct PlayerPettingInfo
{
	public bool isPetting;

	public int npc;

	public int proj;

	public int type;

	public bool mount;

	public Vector2 offsetFromPet;

	public bool isPetSmall;

	public PlayerPettingInfo(NPC npc, Vector2 offsetFromPet, bool isPetSmall)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		isPetting = false;
		this.npc = npc.whoAmI;
		proj = -1;
		type = npc.type;
		this.offsetFromPet = offsetFromPet;
		this.isPetSmall = isPetSmall;
		mount = false;
	}

	public PlayerPettingInfo(Projectile proj, Vector2 offsetFromPet, bool isPetSmall)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		isPetting = false;
		npc = -1;
		this.proj = proj.whoAmI;
		type = proj.type;
		this.offsetFromPet = offsetFromPet;
		this.isPetSmall = isPetSmall;
		mount = false;
	}

	public PlayerPettingInfo(int mountId, bool isPetSmall)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		isPetting = false;
		npc = -1;
		proj = -1;
		type = mountId;
		offsetFromPet = Vector2.Zero;
		this.isPetSmall = isPetSmall;
		mount = true;
	}

	public bool TryGetTarget(out Entity target)
	{
		if (npc >= 0)
		{
			NPC nPC = (NPC)(target = Main.npc[npc]);
			if (nPC.active)
			{
				return nPC.type == type;
			}
			return false;
		}
		if (mount)
		{
			target = null;
			return true;
		}
		Projectile projectile = (Projectile)(target = Main.projectile[proj]);
		if (projectile.active)
		{
			return projectile.type == type;
		}
		return false;
	}
}
