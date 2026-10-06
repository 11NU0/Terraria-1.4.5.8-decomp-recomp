using Terraria.ID;

namespace Terraria.GameContent.ObjectInteractions;

public class BlockBecauseYouAreOverAnImportantTile : ISmartInteractBlockReasonProvider
{
	public bool ShouldBlockSmartInteract(SmartInteractScanSettings settings)
	{
		int tileTargetX = Player.tileTargetX;
		int tileTargetY = Player.tileTargetY;
		if (!WorldGen.InWorld(tileTargetX, tileTargetY, 10))
		{
			return true;
		}
		Tile tile = Main.tile[tileTargetX, tileTargetY];
		if (tile == null)
		{
			return true;
		}
		if (tile.active() && TileID.Sets.DisableSmartInteract[tile.type])
		{
			return true;
		}
		return false;
	}
}
