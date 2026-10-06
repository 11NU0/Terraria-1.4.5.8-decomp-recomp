using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public struct PositionedChest(Chest chest, Vector2 position)
{
	public Chest chest = chest;

	public Vector2 position = position;
}
