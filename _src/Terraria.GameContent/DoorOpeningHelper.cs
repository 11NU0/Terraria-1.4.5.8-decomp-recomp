using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria.GameContent;

public class DoorOpeningHelper
{
	public enum DoorAutoOpeningPreference
	{
		Disabled,
		EnabledForGamepadOnly,
		EnabledForEverything
	}

	private enum DoorCloseAttemptResult
	{
		StillInDoorArea,
		ClosedDoor,
		FailedToCloseDoor,
		DoorIsInvalidated
	}

	private struct DoorOpenCloseTogglingInfo
	{
		public Point tileCoordsForToggling;

		public DoorAutoHandler handler;
	}

	private struct PlayerInfoForOpeningDoors
	{
		public Rectangle hitboxToOpenDoor;

		public int intendedOpeningDirection;

		public int playerGravityDirection;

		public Rectangle tileCoordSpaceForCheckingForDoors;
	}

	private struct PlayerInfoForClosingDoors
	{
		public Rectangle hitboxToNotCloseDoor;
	}

	private interface DoorAutoHandler
	{
		DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords);

		bool TryOpenDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForOpeningDoors playerInfo);

		DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo);
	}

	private class CommonDoorOpeningInfoProvider : DoorAutoHandler
	{
		public DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
			Point tileCoordsForToggling = tileCoords;
			tileCoordsForToggling.Y -= tile.frameY % 54 / 18;
			return new DoorOpenCloseTogglingInfo
			{
				handler = this,
				tileCoordsForToggling = tileCoordsForToggling
			};
		}

		public bool TryOpenDoor(DoorOpenCloseTogglingInfo doorInfo, PlayerInfoForOpeningDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
			int intendedOpeningDirection = playerInfo.intendedOpeningDirection;
			Rectangle val = new Rectangle(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 48);
			switch (playerInfo.playerGravityDirection)
			{
			case 1:
				val.Height += 16;
				break;
			case -1:
				val.Y -= 16;
				val.Height += 16;
				break;
			}
			if (!val.Intersects(playerInfo.hitboxToOpenDoor))
			{
				return false;
			}
			if (playerInfo.hitboxToOpenDoor.Top < val.Top || playerInfo.hitboxToOpenDoor.Bottom > val.Bottom)
			{
				return false;
			}
			WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, intendedOpeningDirection);
			if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != 10)
			{
				NetMessage.SendData(19, -1, -1, null, 0, tileCoordsForToggling.X, tileCoordsForToggling.Y, intendedOpeningDirection);
				return true;
			}
			WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, -intendedOpeningDirection);
			if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != 10)
			{
				NetMessage.SendData(19, -1, -1, null, 0, tileCoordsForToggling.X, tileCoordsForToggling.Y, -intendedOpeningDirection);
				return true;
			}
			return false;
		}

		public DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = info.tileCoordsForToggling;
			Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
			if (!tile.active() || tile.type != 11)
			{
				return DoorCloseAttemptResult.DoorIsInvalidated;
			}
			int num = tile.frameX % 72 / 18;
			Rectangle val = new Rectangle(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 48);
			switch (num)
			{
			case 1:
				val.X -= 16;
				break;
			case 2:
				val.X += 16;
				break;
			}
			val.Inflate(1, 0);
			Rectangle val2 = Rectangle.Intersect(val, playerInfo.hitboxToNotCloseDoor);
			if (val2.Width > 0 || val2.Height > 0)
			{
				return DoorCloseAttemptResult.StillInDoorArea;
			}
			if (WorldGen.CloseDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y))
			{
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				NetMessage.SendData(19, -1, -1, null, 1, tileCoordsForToggling.X, tileCoordsForToggling.Y, 1f);
				return DoorCloseAttemptResult.ClosedDoor;
			}
			return DoorCloseAttemptResult.FailedToCloseDoor;
		}
	}

	private class TallGateOpeningInfoProvider : DoorAutoHandler
	{
		public DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
			Point tileCoordsForToggling = tileCoords;
			tileCoordsForToggling.Y -= tile.frameY % 90 / 18;
			return new DoorOpenCloseTogglingInfo
			{
				handler = this,
				tileCoordsForToggling = tileCoordsForToggling
			};
		}

		public bool TryOpenDoor(DoorOpenCloseTogglingInfo doorInfo, PlayerInfoForOpeningDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
			Rectangle val = new Rectangle(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 80);
			switch (playerInfo.playerGravityDirection)
			{
			case 1:
				val.Height += 16;
				break;
			case -1:
				val.Y -= 16;
				val.Height += 16;
				break;
			}
			if (!val.Intersects(playerInfo.hitboxToOpenDoor))
			{
				return false;
			}
			if (playerInfo.hitboxToOpenDoor.Top < val.Top || playerInfo.hitboxToOpenDoor.Bottom > val.Bottom)
			{
				return false;
			}
			bool flag = false;
			if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag))
			{
				NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), tileCoordsForToggling.X, tileCoordsForToggling.Y);
				return true;
			}
			return false;
		}

		public DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = info.tileCoordsForToggling;
			Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
			if (!tile.active() || tile.type != 389)
			{
				return DoorCloseAttemptResult.DoorIsInvalidated;
			}
			_ = tile.frameY % 90 / 18;
			Rectangle val = new Rectangle(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 80);
			val.Inflate(1, 0);
			Rectangle val2 = Rectangle.Intersect(val, playerInfo.hitboxToNotCloseDoor);
			if (val2.Width > 0 || val2.Height > 0)
			{
				return DoorCloseAttemptResult.StillInDoorArea;
			}
			bool flag = true;
			if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag))
			{
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), tileCoordsForToggling.X, tileCoordsForToggling.Y);
				return DoorCloseAttemptResult.ClosedDoor;
			}
			return DoorCloseAttemptResult.FailedToCloseDoor;
		}
	}

	public static DoorAutoOpeningPreference PreferenceSettings = DoorAutoOpeningPreference.EnabledForEverything;

	private Dictionary<int, DoorAutoHandler> _handlerByTileType = new Dictionary<int, DoorAutoHandler>
	{
		{
			10,
			new CommonDoorOpeningInfoProvider()
		},
		{
			388,
			new TallGateOpeningInfoProvider()
		}
	};

	private List<DoorOpenCloseTogglingInfo> _ongoingOpenDoors = new List<DoorOpenCloseTogglingInfo>();

	private int _timeWeCanOpenDoorsUsingVelocityAlone;

	public void AllowOpeningDoorsByVelocityAloneForATime(int timeInFramesToAllow)
	{
		_timeWeCanOpenDoorsUsingVelocityAlone = timeInFramesToAllow;
	}

	public void Update(Player player)
	{
		LookForDoorsToClose(player);
		if (ShouldTryOpeningDoors())
		{
			LookForDoorsToOpen(player);
		}
		if (_timeWeCanOpenDoorsUsingVelocityAlone > 0)
		{
			_timeWeCanOpenDoorsUsingVelocityAlone--;
		}
	}

	private bool ShouldTryOpeningDoors()
	{
		return PreferenceSettings switch
		{
			DoorAutoOpeningPreference.EnabledForEverything => true, 
			DoorAutoOpeningPreference.EnabledForGamepadOnly => PlayerInput.UsingGamepad, 
			_ => false, 
		};
	}

	public static void CyclePreferences()
	{
		switch (PreferenceSettings)
		{
		case DoorAutoOpeningPreference.Disabled:
			PreferenceSettings = DoorAutoOpeningPreference.EnabledForEverything;
			break;
		case DoorAutoOpeningPreference.EnabledForEverything:
			PreferenceSettings = DoorAutoOpeningPreference.EnabledForGamepadOnly;
			break;
		case DoorAutoOpeningPreference.EnabledForGamepadOnly:
			PreferenceSettings = DoorAutoOpeningPreference.Disabled;
			break;
		}
	}

	public void LookForDoorsToClose(Player player)
	{
		PlayerInfoForClosingDoors playerInfoForClosingDoor = GetPlayerInfoForClosingDoor(player);
		for (int num = _ongoingOpenDoors.Count - 1; num >= 0; num--)
		{
			DoorOpenCloseTogglingInfo info = _ongoingOpenDoors[num];
			if (info.handler.TryCloseDoor(info, playerInfoForClosingDoor) != DoorCloseAttemptResult.StillInDoorArea)
			{
				_ongoingOpenDoors.RemoveAt(num);
			}
		}
	}

	private PlayerInfoForClosingDoors GetPlayerInfoForClosingDoor(Player player)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return new PlayerInfoForClosingDoors
		{
			hitboxToNotCloseDoor = player.Hitbox
		};
	}

	public void LookForDoorsToOpen(Player player)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		PlayerInfoForOpeningDoors playerInfoForOpeningDoor = GetPlayerInfoForOpeningDoor(player);
		if (playerInfoForOpeningDoor.intendedOpeningDirection == 0 && player.velocity.X == 0f)
		{
			return;
		}
		Point tileCoords = default;
		for (int i = playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Left; i <= playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Right; i++)
		{
			for (int j = playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Top; j <= playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Bottom; j++)
			{
				tileCoords.X = i;
				tileCoords.Y = j;
				TryAutoOpeningDoor(tileCoords, playerInfoForOpeningDoor);
			}
		}
	}

	private PlayerInfoForOpeningDoors GetPlayerInfoForOpeningDoor(Player player)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		int num = player.controlRight.ToInt() - player.controlLeft.ToInt();
		int playerGravityDirection = (int)player.gravDir;
		Rectangle hitbox = player.Hitbox;
		hitbox.Y -= -1;
		hitbox.Height += -2;
		float num2 = player.GetAutoDoorVelocityContribution();
		if (num == 0 && _timeWeCanOpenDoorsUsingVelocityAlone == 0)
		{
			num2 = 0f;
		}
		float value = (float)num + num2;
		int num3 = Math.Sign(value) * (int)Math.Ceiling(Math.Abs(value));
		hitbox.X += num3;
		if (num == 0)
		{
			num = Math.Sign(value);
		}
		Rectangle hitbox2;
		Rectangle val = (hitbox2 = player.Hitbox);
		hitbox2.X += num3;
		Rectangle r = Rectangle.Union(val, hitbox2);
		Point val2 = r.TopLeft().ToTileCoordinates();
		Point val3 = r.BottomRight().ToTileCoordinates();
		Rectangle tileCoordSpaceForCheckingForDoors = new Rectangle(val2.X, val2.Y, val3.X - val2.X, val3.Y - val2.Y);
		return new PlayerInfoForOpeningDoors
		{
			hitboxToOpenDoor = hitbox,
			intendedOpeningDirection = num,
			playerGravityDirection = playerGravityDirection,
			tileCoordSpaceForCheckingForDoors = tileCoordSpaceForCheckingForDoors
		};
	}

	private void TryAutoOpeningDoor(Point tileCoords, PlayerInfoForOpeningDoors playerInfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetHandler(tileCoords, out var infoProvider))
		{
			DoorOpenCloseTogglingInfo doorOpenCloseTogglingInfo = infoProvider.ProvideInfo(tileCoords);
			if (infoProvider.TryOpenDoor(doorOpenCloseTogglingInfo, playerInfo))
			{
				_ongoingOpenDoors.Add(doorOpenCloseTogglingInfo);
			}
		}
	}

	private bool TryGetHandler(Point tileCoords, out DoorAutoHandler infoProvider)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		infoProvider = null;
		if (!WorldGen.InWorld(tileCoords.X, tileCoords.Y, 3))
		{
			return false;
		}
		Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
		if (tile == null)
		{
			return false;
		}
		if (!_handlerByTileType.TryGetValue(tile.type, out infoProvider))
		{
			return false;
		}
		return true;
	}
}
