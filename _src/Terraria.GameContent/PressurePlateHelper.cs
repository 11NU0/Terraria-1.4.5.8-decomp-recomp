using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class PressurePlateHelper
{
	public static object EntityCreationLock = new object();

	public static Dictionary<Point, bool[]> PressurePlatesPressed = new Dictionary<Point, bool[]>();

	public static bool NeedsFirstUpdate;

	private static Vector2[] PlayerLastPosition = new Vector2[255];

	private static Rectangle pressurePlateBounds = new Rectangle(0, 0, 16, 10);

	public static void Update()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1 || !NeedsFirstUpdate)
		{
			return;
		}
		foreach (Point key in PressurePlatesPressed.Keys)
		{
			PokeLocation(key);
		}
		PressurePlatesPressed.Clear();
		NeedsFirstUpdate = false;
	}

	public static void Reset()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		PressurePlatesPressed.Clear();
		for (int i = 0; i < PlayerLastPosition.Length; i++)
		{
			PlayerLastPosition[i] = Vector2.Zero;
		}
	}

	public static void ResetPlayer(int player)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Point[] array = PressurePlatesPressed.Keys.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			MoveAwayFrom(array[i], player);
		}
	}

	public static void UpdatePlayerPosition(Player player)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		Point val = new Point(1, 1);
		Vector2 val2 = val.ToVector2();
		List<Point> tilesIn = Collision.GetTilesIn(PlayerLastPosition[player.whoAmI] + val2, PlayerLastPosition[player.whoAmI] + player.Size - val2);
		List<Point> tilesIn2 = Collision.GetTilesIn(player.TopLeft + val2, player.BottomRight - val2);
		Rectangle hitbox = player.Hitbox;
		hitbox.Inflate(-val.X, -val.Y);
		Rectangle hitbox2 = player.Hitbox;
		hitbox2.X = (int)PlayerLastPosition[player.whoAmI].X;
		hitbox2.Y = (int)PlayerLastPosition[player.whoAmI].Y;
		hitbox2.Inflate(-val.X, -val.Y);
		for (int i = 0; i < tilesIn.Count; i++)
		{
			Point val3 = tilesIn[i];
			Tile tile = Main.tile[val3.X, val3.Y];
			if (tile.active() && tile.type == 428)
			{
				pressurePlateBounds.X = val3.X * 16;
				pressurePlateBounds.Y = val3.Y * 16 + 16 - pressurePlateBounds.Height;
				if (!hitbox.Intersects(pressurePlateBounds) && !tilesIn2.Contains(val3))
				{
					MoveAwayFrom(val3, player.whoAmI);
				}
			}
		}
		for (int j = 0; j < tilesIn2.Count; j++)
		{
			Point val4 = tilesIn2[j];
			Tile tile2 = Main.tile[val4.X, val4.Y];
			if (tile2.active() && tile2.type == 428)
			{
				pressurePlateBounds.X = val4.X * 16;
				pressurePlateBounds.Y = val4.Y * 16 + 16 - pressurePlateBounds.Height;
				if (hitbox.Intersects(pressurePlateBounds) && (!tilesIn.Contains(val4) || !hitbox2.Intersects(pressurePlateBounds)))
				{
					MoveInto(val4, player.whoAmI);
				}
			}
		}
		PlayerLastPosition[player.whoAmI] = player.position;
	}

	public static void DestroyPlate(Point location)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (PressurePlatesPressed.TryGetValue(location, out var _))
		{
			PressurePlatesPressed.Remove(location);
			PokeLocation(location);
		}
	}

	private static void UpdatePlatePosition(Point location, int player, bool onIt)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		if (onIt)
		{
			MoveInto(location, player);
		}
		else
		{
			MoveAwayFrom(location, player);
		}
	}

	private static void MoveInto(Point location, int player)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (PressurePlatesPressed.TryGetValue(location, out var value))
		{
			value[player] = true;
			return;
		}
		lock (EntityCreationLock)
		{
			PressurePlatesPressed[location] = new bool[255];
		}
		PressurePlatesPressed[location][player] = true;
		PokeLocation(location);
	}

	private static void MoveAwayFrom(Point location, int player)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (!PressurePlatesPressed.TryGetValue(location, out var value))
		{
			return;
		}
		value[player] = false;
		bool flag = false;
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			lock (EntityCreationLock)
			{
				PressurePlatesPressed.Remove(location);
			}
			PokeLocation(location);
		}
	}

	private static void PokeLocation(Point location)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 1)
		{
			Wiring.blockPlayerTeleportationForOneIteration = true;
			Wiring.HitSwitch(location.X, location.Y);
			NetMessage.SendData(59, -1, -1, null, location.X, location.Y);
		}
	}

	static PressurePlateHelper()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
	}
}
