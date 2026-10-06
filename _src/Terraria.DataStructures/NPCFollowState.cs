using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public class NPCFollowState
{
	private NPC _npc;

	private int? _playerIndexBeingFollowed;

	private Vector2 _floorBreadcrumb;

	public Vector2 BreadcrumbPosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _floorBreadcrumb;
		}
	}

	public bool IsFollowingPlayer => _playerIndexBeingFollowed.HasValue;

	public Player PlayerBeingFollowed
	{
		get
		{
			if (_playerIndexBeingFollowed.HasValue)
			{
				return Main.player[_playerIndexBeingFollowed.Value];
			}
			return null;
		}
	}

	public void FollowPlayer(int playerIndex)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_playerIndexBeingFollowed = playerIndex;
		_floorBreadcrumb = Main.player[playerIndex].Bottom;
		_npc.netUpdate = true;
	}

	public void StopFollowing()
	{
		_playerIndexBeingFollowed = null;
		MoveNPCBackHome();
		_npc.netUpdate = true;
	}

	public void Clear(NPC npcToBelongTo)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_npc = npcToBelongTo;
		_playerIndexBeingFollowed = null;
		_floorBreadcrumb = default;
	}

	private bool ShouldSync()
	{
		return _npc.isLikeATownNPC;
	}

	public void WriteTo(BinaryWriter writer)
	{
		int num = (_playerIndexBeingFollowed.HasValue ? _playerIndexBeingFollowed.Value : (-1));
		writer.Write((short)num);
	}

	public void ReadFrom(BinaryReader reader)
	{
		short num = reader.ReadInt16();
		if (Main.player.IndexInRange(num))
		{
			_playerIndexBeingFollowed = num;
		}
	}

	private void MoveNPCBackHome()
	{
		_npc.ai[0] = 20f;
		_npc.ai[1] = 0f;
		_npc.ai[2] = 0f;
		_npc.ai[3] = 0f;
		_npc.netUpdate = true;
	}

	public void Update()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (IsFollowingPlayer)
		{
			Player playerBeingFollowed = PlayerBeingFollowed;
			if (!playerBeingFollowed.active || playerBeingFollowed.dead)
			{
				StopFollowing();
				return;
			}
			UpdateBreadcrumbs(playerBeingFollowed);
			Dust.QuickDust(_floorBreadcrumb, Color.Red);
		}
	}

	private void UpdateBreadcrumbs(Player player)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Vector2? val = null;
		if (player.velocity.Y == 0f && player.gravDir == 1f)
		{
			val = player.Bottom;
		}
		int num = 8;
		if (val.HasValue && Vector2.Distance(val.Value, _floorBreadcrumb) >= (float)num)
		{
			_floorBreadcrumb = val.Value;
			_npc.netUpdate = true;
		}
	}
}
