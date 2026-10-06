using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent;

public struct PlayerSittingHelper
{
	public const int ChairSittingMaxDistance = 40;

	public bool isSitting;

	public ExtraSeatInfo details;

	public Vector2 offsetForSeat;

	public int sittingIndex;

	public void GetSittingOffsetInfo(Player player, out Vector2 posOffset, out float seatAdjustment)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (isSitting)
		{
			posOffset = new Vector2((float)(sittingIndex * player.direction * 8), (float)sittingIndex * player.gravDir * -4f);
			seatAdjustment = -4f;
			seatAdjustment += (int)offsetForSeat.Y;
			posOffset += offsetForSeat * player.Directions;
		}
		else
		{
			posOffset = Vector2.Zero;
			seatAdjustment = 0f;
		}
	}

	public bool TryGetSittingBlock(Player player, out Tile tile)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		tile = null;
		if (!isSitting)
		{
			return false;
		}
		Point val = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
		if (!GetSittingTargetInfo(player, val.X, val.Y, out var _, out var _, out var _, out var _))
		{
			return false;
		}
		tile = Framing.GetTileSafely(val);
		return true;
	}

	public void UpdateSitting(Player player)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		if (!isSitting)
		{
			return;
		}
		Point val = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
		if (!GetSittingTargetInfo(player, val.X, val.Y, out var targetDirection, out var _, out var seatDownOffset, out var extraInfo))
		{
			SitUp(player);
			return;
		}
		if (player.controlLeft || player.controlRight || player.controlUp || player.controlDown || player.controlJump || player.pulley || player.mount.Active || targetDirection != player.direction)
		{
			SitUp(player);
		}
		if (Main.sittingManager.GetNextPlayerStackIndexInCoords(val) >= 2)
		{
			SitUp(player);
		}
		if (!isSitting)
		{
			return;
		}
		if (Main.netMode != 1 && !Main.IsItDay())
		{
			int num = 2322;
			int num2 = 2358;
			Tile tile = Main.tile[val.X, val.Y];
			if (tile.type == 89 && tile.frameX >= num && tile.frameX <= num2)
			{
				NPC.RedHatSkeletron(player.whoAmI);
			}
		}
		offsetForSeat = seatDownOffset;
		details = extraInfo;
		Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, val, out sittingIndex);
	}

	public void SitUp(Player player, bool multiplayerBroadcast = true)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (isSitting)
		{
			isSitting = false;
			offsetForSeat = Vector2.Zero;
			sittingIndex = -1;
			details = default;
			if (multiplayerBroadcast && Main.myPlayer == player.whoAmI)
			{
				NetMessage.SendData(13, -1, -1, null, player.whoAmI);
			}
		}
	}

	public void SitDown(Player player, int x, int y)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		if (!GetSittingTargetInfo(player, x, y, out var targetDirection, out var playerSittingPosition, out var seatDownOffset, out var extraInfo))
		{
			return;
		}
		Vector2 offset = playerSittingPosition - player.Bottom;
		bool flag = player.CanSnapToPosition(offset);
		if (flag)
		{
			flag &= Main.sittingManager.GetNextPlayerStackIndexInCoords((playerSittingPosition + new Vector2(0f, -2f)).ToTileCoordinates()) < 2;
		}
		if (!flag)
		{
			return;
		}
		if (isSitting && player.Bottom == playerSittingPosition)
		{
			SitUp(player);
			return;
		}
		player.StopVanityActions();
		player.RemoveAllGrapplingHooks();
		if (player.mount.Active)
		{
			player.mount.TryDismount(player);
		}
		player.Bottom = playerSittingPosition;
		player.ChangeDir(targetDirection);
		isSitting = true;
		details = extraInfo;
		offsetForSeat = seatDownOffset;
		Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, new Point(x, y), out sittingIndex);
		player.velocity = Vector2.Zero;
		player.gravDir = 1f;
		if (Main.myPlayer == player.whoAmI)
		{
			NetMessage.SendData(13, -1, -1, null, player.whoAmI);
		}
	}

	public static bool GetSittingTargetInfo(Player player, int x, int y, out int targetDirection, out Vector2 playerSittingPosition, out Vector2 seatDownOffset, out ExtraSeatInfo extraInfo)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		extraInfo = default;
		Tile tileSafely = Framing.GetTileSafely(x, y);
		if (!TileID.Sets.CanBeSatOnForPlayers[tileSafely.type] || !tileSafely.active())
		{
			targetDirection = 1;
			seatDownOffset = Vector2.Zero;
			playerSittingPosition = default;
			return false;
		}
		int num = x;
		int num2 = y;
		targetDirection = 1;
		seatDownOffset = Vector2.Zero;
		int num3 = 6;
		Vector2 zero = Vector2.Zero;
		switch (tileSafely.type)
		{
		case 15:
		case 497:
		{
			bool num6 = tileSafely.type == 15 && (tileSafely.frameY / 40 == 1 || tileSafely.frameY / 40 == 20);
			bool value = tileSafely.type == 15 && tileSafely.frameY / 40 == 27;
			seatDownOffset.Y = value.ToInt() * 4;
			if (tileSafely.frameY % 40 != 0)
			{
				num2--;
			}
			targetDirection = -1;
			if (tileSafely.frameX != 0)
			{
				targetDirection = 1;
			}
			if (num6 || tileSafely.type == 497)
			{
				extraInfo.IsAToilet = true;
			}
			break;
		}
		case 102:
		{
			int num4 = tileSafely.frameX / 18;
			if (num4 == 0)
			{
				num++;
			}
			if (num4 == 2)
			{
				num--;
			}
			int num5 = tileSafely.frameY / 18;
			if (num5 == 0)
			{
				num2 += 2;
			}
			if (num5 == 1)
			{
				num2++;
			}
			if (num5 == 3)
			{
				num2--;
			}
			targetDirection = player.direction;
			num3 = 0;
			break;
		}
		case 487:
		{
			int num7 = tileSafely.frameX % 72 / 18;
			if (num7 == 1)
			{
				num--;
			}
			if (num7 == 2)
			{
				num++;
			}
			if (tileSafely.frameY / 18 != 0)
			{
				num2--;
			}
			targetDirection = (num7 <= 1).ToDirectionInt();
			num3 = 0;
			seatDownOffset.Y--;
			break;
		}
		case 89:
		{
			targetDirection = player.direction;
			num3 = 0;
			Vector2 val = new Vector2(-4f, 2f);
			Vector2 val2 = new Vector2(4f, 2f);
			Vector2 val3 = new Vector2(0f, 2f);
			Vector2 zero2 = Vector2.Zero;
			zero2.X = 1f;
			zero.X = -1f;
			switch (tileSafely.frameX / 54)
			{
			case 0:
				val3.Y = (val.Y = (val2.Y = 1f));
				break;
			case 1:
				val3.Y = 1f;
				break;
			case 2:
			case 14:
			case 15:
			case 17:
			case 20:
			case 21:
			case 22:
			case 23:
			case 25:
			case 26:
			case 27:
			case 28:
			case 35:
			case 37:
			case 38:
			case 39:
			case 40:
			case 41:
			case 42:
				val3.Y = (val.Y = (val2.Y = 1f));
				break;
			case 3:
			case 4:
			case 5:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 16:
			case 18:
			case 19:
			case 36:
				val3.Y = (val.Y = (val2.Y = 0f));
				break;
			case 6:
				val3.Y = (val.Y = (val2.Y = -1f));
				break;
			case 24:
				val3.Y = 0f;
				val.Y = -4f;
				val.X = 0f;
				val2.X = 0f;
				val2.Y = -4f;
				break;
			}
			if (tileSafely.frameY % 40 != 0)
			{
				num2--;
			}
			if ((tileSafely.frameX % 54 == 0 && targetDirection == -1) || (tileSafely.frameX % 54 == 36 && targetDirection == 1))
			{
				seatDownOffset = val;
			}
			else if ((tileSafely.frameX % 54 == 0 && targetDirection == 1) || (tileSafely.frameX % 54 == 36 && targetDirection == -1))
			{
				seatDownOffset = val2;
			}
			else
			{
				seatDownOffset = val3;
			}
			seatDownOffset += zero2;
			break;
		}
		}
		playerSittingPosition = new Point(num, num2 + 1).ToWorldCoordinates(8f, 16f);
		playerSittingPosition.X += targetDirection * num3;
		playerSittingPosition += zero;
		return true;
	}
}
