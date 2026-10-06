using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;

namespace Terraria.GameContent;

public class MinecartDiggerHelper
{
	public static MinecartDiggerHelper Instance = new MinecartDiggerHelper();

	public void TryDigging(Player player, Vector2 trackWorldPosition, int digDirectionX, int digDirectionY)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		digDirectionY = 0;
		Point val = trackWorldPosition.ToTileCoordinates();
		if (Framing.GetTileSafely(val).type != 314 || (double)val.Y < Main.worldSurface)
		{
			return;
		}
		Point val2 = val;
		val2.X += digDirectionX;
		val2.Y += digDirectionY;
		if (AlreadyLeadsIntoWantedTrack(val, val2) || (digDirectionY == 0 && (AlreadyLeadsIntoWantedTrack(val, new Point(val2.X, val2.Y - 1)) || AlreadyLeadsIntoWantedTrack(val, new Point(val2.X, val2.Y + 1)))))
		{
			return;
		}
		int num = 5;
		if (digDirectionY != 0)
		{
			num = 5;
		}
		Point val3 = val2;
		Point val4 = val3;
		val4.Y -= num - 1;
		int x = val4.X;
		for (int i = val4.Y; i <= val3.Y; i++)
		{
			if (!CanGetPastTile(x, i) || !HasPickPower(player, x, i))
			{
				return;
			}
		}
		if (CanConsumeATrackItem(player))
		{
			int x2 = val4.X;
			for (int j = val4.Y; j <= val3.Y; j++)
			{
				MineTheTileIfNecessary(x2, j);
			}
			ConsumeATrackItem(player);
			PlaceATrack(val2.X, val2.Y);
			player.velocity.X = MathHelper.Clamp(player.velocity.X, -1f, 1f);
			if (!DoTheTracksConnectProperly(val, val2))
			{
				CorrectTrackConnections(val, val2);
			}
		}
	}

	private bool CanConsumeATrackItem(Player player)
	{
		return FindMinecartTrackItem(player) != null;
	}

	private void ConsumeATrackItem(Player player)
	{
		Item item = FindMinecartTrackItem(player);
		item.stack--;
		if (item.stack == 0)
		{
			item.TurnToAir();
		}
	}

	private Item FindMinecartTrackItem(Player player)
	{
		Item result = null;
		for (int i = 0; i < 58; i++)
		{
			if (player.selectedItem != i || (player.itemAnimation <= 0 && player.reuseDelay <= 0 && player.itemTime <= 0))
			{
				Item item = player.inventory[i];
				if (item.type == 2340 && item.stack > 0)
				{
					result = item;
					break;
				}
			}
		}
		return result;
	}

	private void PoundTrack(Point spot)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (Main.tile[spot.X, spot.Y].type == 314 && Minecart.FrameTrack(spot.X, spot.Y, pound: true) && Main.netMode == 1)
		{
			NetMessage.SendData(17, -1, -1, null, 15, spot.X, spot.Y, 1f);
		}
	}

	private bool AlreadyLeadsIntoWantedTrack(Point tileCoordsOfFrontWheel, Point tileCoordsWeWantToReach)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Tile tileSafely = Framing.GetTileSafely(tileCoordsOfFrontWheel);
		Tile tileSafely2 = Framing.GetTileSafely(tileCoordsWeWantToReach);
		if (!tileSafely.active() || tileSafely.type != 314)
		{
			return false;
		}
		if (!tileSafely2.active() || tileSafely2.type != 314)
		{
			return false;
		}
		GetExpectedDirections(tileCoordsOfFrontWheel, tileCoordsWeWantToReach, out var expectedStartLeft, out var expectedStartRight, out var expectedEndLeft, out var expectedEndRight);
		if (!Minecart.GetAreExpectationsForSidesMet(tileCoordsOfFrontWheel, expectedStartLeft, expectedStartRight))
		{
			return false;
		}
		if (!Minecart.GetAreExpectationsForSidesMet(tileCoordsWeWantToReach, expectedEndLeft, expectedEndRight))
		{
			return false;
		}
		return true;
	}

	private static void GetExpectedDirections(Point startCoords, Point endCoords, out int? expectedStartLeft, out int? expectedStartRight, out int? expectedEndLeft, out int? expectedEndRight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		int num = endCoords.Y - startCoords.Y;
		int num2 = endCoords.X - startCoords.X;
		expectedStartLeft = null;
		expectedStartRight = null;
		expectedEndLeft = null;
		expectedEndRight = null;
		if (num2 == -1)
		{
			expectedStartLeft = num;
			expectedEndRight = -num;
		}
		if (num2 == 1)
		{
			expectedStartRight = num;
			expectedEndLeft = -num;
		}
	}

	private bool DoTheTracksConnectProperly(Point tileCoordsOfFrontWheel, Point tileCoordsWeWantToReach)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return AlreadyLeadsIntoWantedTrack(tileCoordsOfFrontWheel, tileCoordsWeWantToReach);
	}

	private void CorrectTrackConnections(Point startCoords, Point endCoords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		GetExpectedDirections(startCoords, endCoords, out var expectedStartLeft, out var expectedStartRight, out var expectedEndLeft, out var expectedEndRight);
		Tile tileSafely = Framing.GetTileSafely(startCoords);
		Tile tileSafely2 = Framing.GetTileSafely(endCoords);
		if (tileSafely.active() && tileSafely.type == 314)
		{
			Minecart.TryFittingTileOrientation(startCoords, expectedStartLeft, expectedStartRight);
		}
		if (tileSafely2.active() && tileSafely2.type == 314)
		{
			Minecart.TryFittingTileOrientation(endCoords, expectedEndLeft, expectedEndRight);
		}
	}

	private bool HasPickPower(Player player, int x, int y)
	{
		if (player.HasEnoughPickPowerToHurtTile(x, y))
		{
			return true;
		}
		return false;
	}

	private bool CanGetPastTile(int x, int y)
	{
		if (WorldGen.CheckTileBreakability(x, y) != 0)
		{
			return false;
		}
		if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(x, y))
		{
			return false;
		}
		Tile tile = Main.tile[x, y];
		if (tile.active() && ((tile.type == 26 && !Main.hardMode) || !WorldGen.CanKillTile(x, y)))
		{
			return false;
		}
		return true;
	}

	private void PlaceATrack(int x, int y)
	{
		int num = 314;
		int num2 = 0;
		if (WorldGen.PlaceTile(x, y, num, mute: false, forced: false, Main.myPlayer, num2))
		{
			NetMessage.SendData(17, -1, -1, null, 1, x, y, num, num2);
		}
	}

	private void MineTheTileIfNecessary(int x, int y)
	{
		AchievementsHelper.CurrentlyMining = true;
		if (Main.tile[x, y].active())
		{
			WorldGen.KillTile(x, y);
			NetMessage.SendData(17, -1, -1, null, 0, x, y);
		}
		AchievementsHelper.CurrentlyMining = false;
	}
}
