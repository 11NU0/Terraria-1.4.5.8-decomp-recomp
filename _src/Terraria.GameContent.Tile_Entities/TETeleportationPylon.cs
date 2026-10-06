using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities;

public class TETeleportationPylon : TileEntityType<TETeleportationPylon>
{
	private const int MyTileID = 597;

	public const int entityTileWidth = 3;

	public const int entityTileHeight = 4;

	public override void NetPlaceEntityAttempt(int x, int y)
	{
		if (!TryGetPylonTypeFromTileCoords(x, y, out var pylonType))
		{
			RejectPlacementFromNet(x, y);
		}
		else if (Main.PylonSystem.HasPylonOfType(pylonType))
		{
			RejectPlacementFromNet(x, y);
		}
		else
		{
			base.NetPlaceEntityAttempt(x, y);
		}
	}

	public bool TryGetPylonType(out TeleportPylonType pylonType)
	{
		return TryGetPylonTypeFromTileCoords(Position.X, Position.Y, out pylonType);
	}

	private static void RejectPlacementFromNet(int x, int y)
	{
		WorldGen.KillTile(x, y);
		if (Main.netMode == 2)
		{
			NetMessage.SendData(17, -1, -1, null, 0, x, y);
		}
	}

	public override void OnPlaced()
	{
		Main.PylonSystem.RequestImmediateUpdate();
	}

	public override void OnRemoved()
	{
		Main.PylonSystem.RequestImmediateUpdate();
	}

	public override string ToString()
	{
		return Position.X + "x  " + Position.Y + "y";
	}

	public static void Framing_CheckTile(int callX, int callY)
	{
		if (WorldGen.destroyObject)
		{
			return;
		}
		int num = callX;
		int num2 = callY;
		Tile tileSafely = Framing.GetTileSafely(callX, callY);
		num -= tileSafely.frameX / 18 % 3;
		num2 -= tileSafely.frameY / 18 % 4;
		int pylonStyleFromTile = GetPylonStyleFromTile(tileSafely);
		bool flag = false;
		for (int i = num; i < num + 3; i++)
		{
			for (int j = num2; j < num2 + 4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null)
				{
					return;
				}
				if (!tile.active() || tile.type != 597)
				{
					flag = true;
				}
			}
		}
		if (!WorldGen.SolidTileAllowBottomSlope(num, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 1, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 2, num2 + 4))
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		TileEntityType<TETeleportationPylon>.Kill(num, num2);
		int pylonItemTypeFromTileStyle = GetPylonItemTypeFromTileStyle(pylonStyleFromTile);
		Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 48, 64, pylonItemTypeFromTileStyle);
		WorldGen.destroyObject = true;
		for (int k = num; k < num + 3; k++)
		{
			for (int l = num2; l < num2 + 4; l++)
			{
				if (Main.tile[k, l].active() && Main.tile[k, l].type == 597)
				{
					WorldGen.KillTile(k, l);
				}
			}
		}
		WorldGen.destroyObject = false;
	}

	public static int GetPylonStyleFromTile(Tile tile)
	{
		return tile.frameX / 54;
	}

	public static int GetPylonItemTypeFromTileStyle(int style)
	{
		return style switch
		{
			1 => 4875, 
			2 => 4916, 
			3 => 4917, 
			4 => 4918, 
			5 => 4919, 
			6 => 4920, 
			7 => 4921, 
			8 => 4951, 
			9 => 5652, 
			10 => 5653, 
			_ => 4876, 
		};
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 597 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 54 != 0)
		{
			return false;
		}
		return true;
	}

	public static int PlacementPreviewHook_AfterPlacement(int x, int y, int type = 597, int style = 0, int direction = 1, int alternate = 0)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 3, 3, 4);
			NetMessage.SendData(87, -1, -1, null, x + -1, y + -3, (int)TileEntityType<TETeleportationPylon>.EntityTypeID);
			return -1;
		}
		return TileEntityType<TETeleportationPylon>.Place(x + -1, y + -3);
	}

	public static int PlacementPreviewHook_CheckIfCanPlace(int x, int y, int type = 597, int style = 0, int direction = 1, int alternate = 0)
	{
		TeleportPylonType pylonTypeFromPylonTileStyle = GetPylonTypeFromPylonTileStyle(style);
		if (Main.PylonSystem.HasPylonOfType(pylonTypeFromPylonTileStyle))
		{
			return 1;
		}
		return 0;
	}

	private bool TryGetPylonTypeFromTileCoords(int x, int y, out TeleportPylonType pylonType)
	{
		pylonType = TeleportPylonType.SurfacePurity;
		Tile tile = Main.tile[x, y];
		if (tile == null || !tile.active() || tile.type != 597)
		{
			return false;
		}
		int pylonStyle = tile.frameX / 54;
		pylonType = GetPylonTypeFromPylonTileStyle(pylonStyle);
		return true;
	}

	private static TeleportPylonType GetPylonTypeFromPylonTileStyle(int pylonStyle)
	{
		return (TeleportPylonType)pylonStyle;
	}
}
