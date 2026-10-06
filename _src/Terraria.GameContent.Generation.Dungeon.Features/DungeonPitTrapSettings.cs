using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonPitTrapSettings : DungeonFeatureSettings
{
	public DungeonGenerationStyleData Style;

	public int Width;

	public int Height;

	public int EdgeWidth;

	public int EdgeHeight;

	public int TopDensity;

	public bool Flooded;

	public DungeonRoom ConnectedRoom;
}
