using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding;

public struct LandmassData
{
	public LandmassDataType DataType;

	public Vector2 Position;

	public int RadiusOrHalfSize;

	public int Style;

	public int ExtraData;

	public Vector2 Top
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return Position - new Vector2(0f, (float)RadiusOrHalfSize);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Position = value + new Vector2(0f, (float)RadiusOrHalfSize);
		}
	}
}
